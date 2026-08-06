using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;

public class FloorShape : MonoBehaviour
{
    Grid grid;
    public List<Vector2Int> shapeCells;

    //the leftmost and rightmost cell
    private Vector2Int MinX;
    private Vector2Int MaxX;

    private Vector2Int MinY;
    private Vector2Int MaxY;

    public void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
        if (grid == null)
        {
            Debug.Log("Floor shape could not find the grid.");
        }

        if (shapeCells.Count > 0)
        {

            foreach (var cell in shapeCells)
            {
                if (cell.x < MinX.x)
                {
                    MinX = cell;
                }
                if (cell.x > MaxX.x)
                {
                    MaxX = cell;
                }
                if (cell.y < MinY.y)
                {
                    MinY = cell;
                }
                if (cell.y > MaxY.y)
                {
                    MaxY = cell;
                }
            }
        }
    }


    public List<Vector2Int> GetFloorCells()
    {
        Vector2Int origin = GetOriginCell();
        return shapeCells.Select(coord => origin + coord).ToList();
    }

    public List<Vector2Int> GetFloorCellsWithOrigin(Vector2Int origin)
    {
        return shapeCells.Select(coord => origin + coord).ToList();
    }

    public Vector2Int GetMinXWithOrigin(Vector2Int origin)
    {
        return MinX + origin;
    }

    public Vector2Int GetMinYWithOrigin(Vector2Int origin)
    {
        return MinY + origin;
    }
    public Vector2Int GetMaxXWithOrigin(Vector2Int origin)
    {
        return MaxX + origin;
    }
    public Vector2Int GetMaxYWithOrigin(Vector2Int origin)
    {
        return MaxY + origin;
    }

    private Vector2Int GetOriginCell()
    {
        Vector3 worldOrigin = grid.WorldToCell(gameObject.transform.position);
        Vector2Int origin = new Vector2Int((int)worldOrigin.x, (int)worldOrigin.y);
        return origin;
    }

    private void OnDrawGizmosSelected()
    {
        if (grid == null)
        {
            grid = FindAnyObjectByType<Grid>();
        }
        Vector2Int origin = GetOriginCell();

        List<Vector2Int> gridCoordinates = shapeCells.Select(coord => origin + coord).ToList<Vector2Int>();

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