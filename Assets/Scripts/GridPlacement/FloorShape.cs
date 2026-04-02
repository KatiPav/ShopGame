using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FloorShape : MonoBehaviour
{
    public List<Vector2Int> shapeCells;

    private void OnDrawGizmosSelected()
    {
        if (shapeCells == null) return;


        Grid grid = FindAnyObjectByType<Grid>();
        if (grid == null) return;

        Vector3 worldOrigin = grid.WorldToCell(gameObject.transform.position);
        Vector2Int origin = new Vector2Int((int)worldOrigin.x, (int)worldOrigin.y);
        List<Vector2Int> gridCoordinates = shapeCells.Select(coord => origin + coord).ToList<Vector2Int>();
        gridCoordinates.Add(origin);
        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.8f);
        foreach (Vector2Int coord in gridCoordinates)
        {
            Vector3 center = grid.GetCellCenterWorld(new Vector3Int(coord.x, coord.y, 0));

            float halfW = grid.cellSize.x * 0.5f;
            float halfH = grid.cellSize.y * 0.5f;

            // 4 corners of an isometric diamond in 2D screen space
            Vector3 top = center + new Vector3(0, halfH, 0);
            Vector3 bottom = center + new Vector3(0, -halfH, 0);
            Vector3 left = center + new Vector3(-halfW, 0, 0);
            Vector3 right = center + new Vector3(halfW, 0, 0);

            Gizmos.DrawLine(top, right);
            Gizmos.DrawLine(right, bottom);
            Gizmos.DrawLine(bottom, left);
            Gizmos.DrawLine(left, top);
        }
    }
}