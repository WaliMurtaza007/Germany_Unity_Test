using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Refs")]
    public GridSystem grid;
    public List<ElementDefinition> elements;
    public LayerMask placementMask = ~0;
    public BasicPlacementRule placementRule;

    [Header("State")]
    public ElementType selected = ElementType.Pole;
    public Dictionary<Vector3Int, List<ElementInstance>> occupied = new(); // stack per cell

    private Camera _cam;
    private GhostPreview _ghost;
    Dictionary<Vector3Int, List<ElementInstance>> stacks = new();

    void Awake()
    {
        _cam = Camera.main;
        _ghost = GetComponent<GhostPreview>();
    }

    public ElementDefinition GetDef(ElementType t) => elements.Find(e => e.Type == t);

    void Update()
    {
        HandleHotkeys();
        DoPlacementLoop();
    }

    void HandleHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) selected = ElementType.Pole;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selected = ElementType.Platform;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selected = ElementType.Connector;
        if (Input.GetKeyDown(KeyCode.S)) FindObjectOfType<SaveLoadService>()?.Save();
        if (Input.GetKeyDown(KeyCode.L)) FindObjectOfType<SaveLoadService>()?.Load();
    }

    void DoPlacementLoop()
    {
        if (!_cam) return;
        var ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 500f, placementMask))
        {
            // Snap to grid as fallback
            var snapped = grid.Snap(hit.point);
            var cell = grid.WorldToCell(snapped);

            // 👇 Check if we hit an ElementInstance
            var hitInstance = hit.collider.GetComponentInParent<ElementInstance>();
            if (hitInstance != null)
            {
                // Use that instance's cell for stacking
                cell = hitInstance.Cell;

                // Look up stack for that cell
                if (!stacks.TryGetValue(cell, out var list))
                    list = new List<ElementInstance>();

                float stackHeight = 0f;
                foreach (var e in list) stackHeight += e.Definition.height;

                // Adjust snapped position to top of stack
                snapped = grid.CellToWorld(cell) + new Vector3(0, stackHeight, 0);
            }

            var rot = _ghost.CurrentRotation;
            var def = GetDef(selected);

            // Find height based on stack in this cell
            Vector3 previewPos = GetSnapPosition(def, cell);

            bool canPlace = CanPlace(def, cell, out var reason);
            if (_ghost) _ghost.Show(def, previewPos, rot, canPlace);

            if (Input.GetKeyDown(KeyCode.R)) _ghost.Rotate90();

            if (Input.GetMouseButtonDown(0)) // left click to place
            {
                if (canPlace)
                {
                    PlaceValid(def, cell, rot);
                }
                else
                {
                    PlaceInvalid(def, previewPos, rot);
                    Debug.Log("Invalid placement → will fall: " + reason);
                }
            }
            else if (Input.GetMouseButtonDown(1)) // right click to remove
            {
                if (occupied.TryGetValue(cell, out var stack) && stack.Count > 0)
                {
                    var inst = stack[stack.Count - 1];
                    Remove(inst);
                }
            }
        }
    }

    // ---------- Placement ----------

    ElementInstance PlaceValid(ElementDefinition def, Vector3Int cell, Quaternion rot)
    {
        if (!occupied.TryGetValue(cell, out var stack))
        {
            stack = new List<ElementInstance>();
            occupied[cell] = stack;
        }

        Vector3 worldPos = GetSnapPosition(def, cell);

        var go = Instantiate(def.Prefab, worldPos, rot);
        var inst = go.GetComponent<ElementInstance>();
        inst.Init(def, cell, rot);


        // find belowDef
        ElementDefinition belowDef = stack.Count > 0 ? stack[stack.Count - 1].Definition : null;
        bool stable = placementRule.IsValidPlacement(def, belowDef);
        inst.SetStable(stable);
        if (!stacks.ContainsKey(cell))
            stacks[cell] = new List<ElementInstance>();

        stacks[cell].Add(inst);

        stack.Add(inst);
        return inst;
    }

    void PlaceInvalid(ElementDefinition def, Vector3 worldPos, Quaternion rot)
    {
        var go = Instantiate(def.Prefab, worldPos, rot);
        var rb = go.GetComponent<Rigidbody>() ?? go.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void Remove(ElementInstance inst)
    {
        foreach (var kv in occupied)
        {
            if (kv.Value.Contains(inst))
            {
                kv.Value.Remove(inst);
                break;
            }
        }
        if (stacks.TryGetValue(inst.Cell, out var list))
        {
            list.Remove(inst);
            if (list.Count == 0)
                stacks.Remove(inst.Cell);
        }

        Destroy(inst.gameObject);
    }

    bool CanPlace(ElementDefinition def, Vector3Int cell, out string reason)
    {
        reason = null;
        if (!occupied.TryGetValue(cell, out var stack) || stack.Count == 0)
        {
            if (placementRule.IsValidPlacement(def, null)) return true;
            reason = "Invalid base.";
            return false;
        }

        var below = stack[stack.Count - 1].Definition;
        if (placementRule.IsValidPlacement(def, below)) return true;

        reason = $"Cannot place {def.Type} on {below.Type}";
        return false;
    }

    Vector3 GetSnapPosition(ElementDefinition def, Vector3Int cell)
    {
        float snapHeight = 0f;
        if (occupied.TryGetValue(cell, out var stack))
        {
            foreach (var inst in stack)
                snapHeight += inst.Definition.height;
        }

        return grid.CellToWorld(cell) + new Vector3(0, snapHeight, 0) + def.placementOffset;
    }

    // ---------- Save/Load ----------

    public BuildSave GetSaveData()
    {
        var save = new BuildSave();
        foreach (var kv in occupied)
        {
            foreach (var inst in kv.Value)
            {
                save.items.Add(new ElementSave
                {
                    type = (int)inst.Definition.Type,
                    cell = kv.Key,
                    rotEuler = inst.transform.rotation.eulerAngles
                });
            }
        }
        return save;
    }

    public void LoadFromSave(BuildSave save)
    {
        foreach (var kv in occupied)
            foreach (var inst in kv.Value)
                Destroy(inst.gameObject);

        occupied.Clear();

        foreach (var item in save.items)
        {
            var def = GetDef((ElementType)item.type);
            PlaceValid(def, item.cell, Quaternion.Euler(item.rotEuler));
        }
    }
}

[Serializable]
public class ElementSave
{
    public int type;
    public Vector3Int cell;
    public Vector3 rotEuler;
}

[Serializable]
public class BuildSave
{
    public List<ElementSave> items = new List<ElementSave>();
}
