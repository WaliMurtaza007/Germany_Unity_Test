using UnityEngine;

public class GhostPreview : MonoBehaviour
{
    public Material validMat;
    public Material invalidMat;

    private GameObject _ghost;
    private Quaternion _rot = Quaternion.identity;
    public GameObject CurrentGhost { get; private set; }

    public Quaternion CurrentRotation => _rot;

    public void Rotate90() => _rot = Quaternion.Euler(0, 90, 0) * _rot;

    public void Show(ElementDefinition def, Vector3 pos, Quaternion rot, bool valid)
    {
        if (def == null) return;

        if (_ghost == null || _ghost.name != def.Prefab.name + "(Ghost)")
        {
            if (_ghost) Destroy(_ghost);
            _ghost = Instantiate(def.Prefab);
            _ghost.name = def.Prefab.name + "(Ghost)";
            CurrentGhost = _ghost;

            foreach (var c in _ghost.GetComponentsInChildren<Collider>()) c.enabled = false;
            foreach (var rb in _ghost.GetComponentsInChildren<Rigidbody>()) Destroy(rb);
        }

        _rot = rot;
        _ghost.transform.SetPositionAndRotation(pos, rot);

        var mat = valid ? validMat : invalidMat;
        if (mat != null)
        {
            foreach (var r in _ghost.GetComponentsInChildren<Renderer>())
                r.sharedMaterial = mat;
        }
    }

    void OnDisable()
    {
        if (_ghost) Destroy(_ghost);
    }
}
