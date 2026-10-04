using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShapePrint : MonoBehaviour
{
    protected Grid grid;
    public List<Vector2Int> shapeCells;

    //the leftmost and rightmost cell
    private Vector2Int MinX;
    private Vector2Int MaxX;

    private Vector2Int MinY;
    private Vector2Int MaxY;

    public void Awake()
    {

        grid = GetGrid();

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

    virtual public Grid GetGrid()
    {
        Grid gr = FindAnyObjectByType<Grid>();
        if (gr == null)
        {
            Debug.Log("Default grid could not be found for shape print.");
        }

        Debug.Log("Default grid is chosen for a shape print. This is unusual.");
        return gr;
    }


    public List<Vector2Int> GetCells()
    {
        Vector2Int origin = GetOriginCell();
        return shapeCells.Select(coord => origin + coord).ToList();
    }

    public List<Vector2Int> GetCellsWithOrigin(Vector2Int origin)
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

    protected Vector2Int GetOriginCell()
    {
        Vector3 worldOrigin = grid.WorldToCell(gameObject.transform.position);
        Vector2Int origin = new Vector2Int((int)worldOrigin.x, (int)worldOrigin.y);
        return origin;
    }

    public virtual void OnDrawGizmosSelected()
    {
        Debug.Log("huh??");
    }
}