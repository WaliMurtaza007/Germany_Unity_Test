using UnityEngine;

public enum ElementType
{
    Platform,
    Pole,
    Connector
}

[CreateAssetMenu(menuName = "Scaffolding/ElementDefinition")]
public class ElementDefinition : ScriptableObject
{
    public ElementType Type;
    public GameObject Prefab;

    [Tooltip("Offset for snapping inside the grid cell")]
    public Vector3 placementOffset;

    [Tooltip("Height of this element (used for stacking)")]
    public float height = 1f;
}
