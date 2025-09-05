
using UnityEngine;

public interface IBuildPlacementRule
{
    public bool CanPlace(ElementDefinition def, Vector3Int cell, Quaternion rot, BuildManager ctx, out string reason);
}
