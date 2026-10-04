using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WallShape : ShapePrint
{
    public override Grid GetGrid()
    {
        GameObject gridObj = GameObject.Find("LeftWallGrid");
        Grid gr = gridObj?.GetComponent<Grid>();
        if (gr == null)
        {
            Debug.Log("Wall shape could not find the grid.");

        }
        return gr;
    }

    public override void OnDrawGizmosSelected()
    {
        if (grid == null)
        {
            GameObject go = GameObject.Find("LeftWallGrid");
            if (go != null) grid = go.GetComponent<Grid>();
        }
        if (grid == null) return;

        Vector2Int origin = GetOriginCell();

        Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.8f);
        foreach (Vector2Int coord in shapeCells)
        {
            Vector2Int cell = origin + coord;

            // Rectangle layout: CellToWorld returns the cell's bottom-left corner.
            // The skew parents are applied automatically.
            Vector3 bottomLeft = grid.CellToWorld(new Vector3Int(cell.x, cell.y, 0));
            Vector3 bottomRight = grid.CellToWorld(new Vector3Int(cell.x + 1, cell.y, 0));
            Vector3 topRight = grid.CellToWorld(new Vector3Int(cell.x + 1, cell.y + 1, 0));
            Vector3 topLeft = grid.CellToWorld(new Vector3Int(cell.x, cell.y + 1, 0));

            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);
        }
    }
}