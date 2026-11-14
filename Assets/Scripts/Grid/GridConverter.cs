using UnityEngine;
public class GridConverter : MonoBehaviour
{
    [SerializeField]
    Grid floorGrid;

    [SerializeField]
    Grid leftWallGrid;

    [SerializeField]
    Grid rightWallGrid;

    private Grid grid;

    void Awake()
    {
        grid = floorGrid;
    }

    private void ChangeGrid(PlaneCategory category)
    {
        if (category == PlaneCategory.LeftWall)
        {
            grid = leftWallGrid;
        }

        if (category == PlaneCategory.Floor)
        {
            grid = floorGrid;
        }

        if (category == PlaneCategory.RightWall)
        {
            grid = rightWallGrid;
        }
    }

    public Vector3 GridCoordsToWorldCoords(Vector2Int coords, PlaneCategory category = PlaneCategory.Floor)
    {
        ChangeGrid(category);
        return grid.GetCellCenterWorld(new Vector3Int(coords.x, coords.y, 0));
    }

    public Vector2Int WorldCoordsToGridCoords(Vector3 coords, PlaneCategory category = PlaneCategory.Floor)
    {
        ChangeGrid(category);
        Vector3Int coordinates = grid.WorldToCell(coords);
        return new Vector2Int(coordinates.x, coordinates.y);
    }

    public Vector2Int GetGridCoordinatesOfMousePosition(PlaneCategory category = PlaneCategory.Floor)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return WorldCoordsToGridCoords(worldPos, category);
    }

}