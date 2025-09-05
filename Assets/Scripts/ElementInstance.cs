using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ElementInstance : MonoBehaviour
{
    public ElementDefinition Definition { get; private set; }

    // 🔹 Add these so BuildManager can use them
    public Vector3Int Cell { get; private set; }
    public Quaternion Rot { get; private set; }

    private Rigidbody rb;

    public void Init(ElementDefinition def, Vector3Int cell, Quaternion rot)
    {
        Definition = def;
        Cell = cell;
        Rot = rot;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // start kinematic, physics disabled
    }

    public void SetStable(bool stable)
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        if (stable)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}
