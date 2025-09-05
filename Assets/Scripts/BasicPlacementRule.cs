using UnityEngine;

[CreateAssetMenu(menuName = "Scaffolding/BasicPlacementRule")]
public class BasicPlacementRule : ScriptableObject
{
    public ElementType platformBase;
    public ElementType poleBase;
    public ElementType connectorBase;
    public bool IsValidPlacement(ElementDefinition def, ElementDefinition below)
    {
        if (def == null) return false;
        if (below == null) // ground case
        {
            return def.Type == ElementType.Platform;
        }

        switch (def.Type)
        {
            case ElementType.Platform:
                return below.Type == platformBase;

            case ElementType.Pole:
                return below.Type == poleBase;

            case ElementType.Connector:
                return below.Type == connectorBase;

            default:
                return false;
        }
    }
}
