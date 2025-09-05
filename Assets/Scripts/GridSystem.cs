/*
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [Header("Grid Settings")] public float cellSize = 1f;
    public int maxX = 100, maxY = 50, maxZ = 100; // soft bounds


    public Vector3 Snap(Vector3 worldPos)
    {
        var p = worldPos / cellSize;
        p = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), Mathf.Round(p.z));
        return p * cellSize;
    }


    public Vector3Int WorldToCell(Vector3 world) => new Vector3Int(
    Mathf.RoundToInt(world.x / cellSize),
    Mathf.RoundToInt(world.y / cellSize),
    Mathf.RoundToInt(world.z / cellSize));


    public Vector3 CellToWorld(Vector3Int cell) => new Vector3(
    cell.x * cellSize, cell.y * cellSize, cell.z * cellSize);


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.1f);
        int gx = Mathf.Min(maxX, 50), gz = Mathf.Min(maxZ, 50);
        for (int x = -gx; x <= gx; x++)
            for (int z = -gz; z <= gz; z++)
            {
                Vector3 a = new Vector3(x, 0, z) * cellSize;
                Vector3 b = a + new Vector3(cellSize, 0, 0);
                Vector3 c = a + new Vector3(0, 0, cellSize);
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(a, c);
            }
    }
}
*/
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public float cellSize = 1f;

    public Vector3 Snap(Vector3 worldPos)
    {
        var p = worldPos / cellSize;
        p = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), Mathf.Round(p.z));
        return p * cellSize;
    }

    public Vector3Int WorldToCell(Vector3 world) => new Vector3Int(
        Mathf.RoundToInt(world.x / cellSize),
        Mathf.RoundToInt(world.y / cellSize),
        Mathf.RoundToInt(world.z / cellSize));

    public Vector3 CellToWorld(Vector3Int cell) => new Vector3(
        cell.x * cellSize, cell.y * cellSize, cell.z * cellSize);
}
