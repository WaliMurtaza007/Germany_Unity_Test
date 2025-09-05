using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilitySolver : MonoBehaviour
{
    public BuildManager build;
    private bool _dirty;

  /*  public void EnqueueRecheck() => _dirty = true;

    void LateUpdate()
    {
        if (!_dirty || build == null) return;
        _dirty = false;
        RecomputeSupport();
    }

    void RecomputeSupport()
    {
        var supported = new HashSet<Vector3Int>();
        var q = new Queue<Vector3Int>();

        // Seed: everything at y==0
        foreach (var kv in build.occupied)
        {
            if (kv.Key.y == 0)
            {
                supported.Add(kv.Key);
                q.Enqueue(kv.Key);
            }
        }

        while (q.Count > 0)
        {
            var c = q.Dequeue();
            if (!build.occupied.TryGetValue(c, out var inst)) continue;

            foreach (var n in GetRelevantNeighbors(c, inst.Type))
            {
                if (!build.occupied.ContainsKey(n) || supported.Contains(n)) continue;
                if (IsStructurallyConnected(c, n))
                {
                    supported.Add(n);
                    q.Enqueue(n);
                }
            }
        }

        // Apply kinematic/dynamic based on support
        foreach (var kv in build.occupied)
        {
            bool shouldBeKinematic = supported.Contains(kv.Key);
            kv.Value.SetKinematic(shouldBeKinematic);
        }
    }

    bool IsStructurallyConnected(Vector3Int a, Vector3Int b)
    {
        if (!build.occupied.TryGetValue(a, out var ia) || !build.occupied.TryGetValue(b, out var ib)) return false;
        if (a == b) return true;

        // Pole <-> Pole vertical stacking
        if (ia.Type == ElementType.Pole && ib.Type == ElementType.Pole)
        {
            if (b == a + Vector3Int.up || b == a + Vector3Int.down) return true;
            if (IsPoleLateralBraced(a, b)) return true;
        }

        // Pole <-> Platform
        if (ia.Type == ElementType.Platform && ib.Type == ElementType.Pole)
            return PoleSupportsPlatform(ib.Cell, ia.Cell);
        if (ia.Type == ElementType.Pole && ib.Type == ElementType.Platform)
            return PoleSupportsPlatform(ia.Cell, ib.Cell);

        // Platform <-> Platform (adjacent on same layer)
        if (ia.Type == ElementType.Platform && ib.Type == ElementType.Platform)
            return (a.y == b.y) && (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z) == 1);

        // Connector <-> Pole adjacency
        if (ia.Type == ElementType.Connector && ib.Type == ElementType.Pole) return AreNeighbors(a, b);
        if (ia.Type == ElementType.Pole && ib.Type == ElementType.Connector) return AreNeighbors(a, b);

        // Connector <-> Connector chain
        if (ia.Type == ElementType.Connector && ib.Type == ElementType.Connector)
            return (a.y == b.y) && (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z) == 1);

        return false;
    }

    bool PoleSupportsPlatform(Vector3Int pole, Vector3Int platform)
    {
        if (platform.y - pole.y != 1) return false;
        Vector3Int[] corners =
        {
                platform + new Vector3Int(1,-1,1),
                platform + new Vector3Int(-1,-1,-1),
                platform + new Vector3Int(1,-1,-1),
                platform + new Vector3Int(-1,-1,1)
            };
        foreach (var c in corners) if (c == pole) return true;
        return false;
    }

    bool AreNeighbors(Vector3Int a, Vector3Int b)
        => (a.y == b.y) && (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z) == 1);

    bool IsPoleLateralBraced(Vector3Int a, Vector3Int b)
    {
        if (a.y != b.y) return false;
        if (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z) != 1) return false;

        // look for a connector cell that touches both poles
        // check neighbors around a and b for connector that is neighbor to both
        Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.forward, Vector3Int.back };

        foreach (var d in directions)
        {
            var cand = a + d;
            if (!build.occupied.TryGetValue(cand, out var inst)) continue;
            if (inst.Type != ElementType.Connector) continue;
            if (AreNeighbors(cand, b)) return true;
        }

        foreach (var d in directions)
        {
            var cand = b + d;
            if (!build.occupied.TryGetValue(cand, out var inst)) continue;
            if (inst.Type != ElementType.Connector) continue;
            if (AreNeighbors(cand, a)) return true;
        }

        return false;
    }

    IEnumerable<Vector3Int> GetRelevantNeighbors(Vector3Int cell, ElementType type)
    {
        yield return cell + Vector3Int.up;
        yield return cell + Vector3Int.down;

        yield return cell + Vector3Int.right;
        yield return cell + Vector3Int.left;
        yield return cell + Vector3Int.forward;
        yield return cell + Vector3Int.back;

        if (type == ElementType.Platform)
        {
            // poles under corners matter
            yield return cell + new Vector3Int(1, -1, 1);
            yield return cell + new Vector3Int(-1, -1, -1);
            yield return cell + new Vector3Int(1, -1, -1);
            yield return cell + new Vector3Int(-1, -1, 1);
        }
    }*/
}
