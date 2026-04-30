using UnityEngine;
public class GridCoordinates : MonoBehaviour
{
    [SerializeField]
    Grid grid;

    public Vector3 GridCoordsToWorldCoords(Vector2Int coords)
    {
        return grid.GetCellCenterWorld(new Vector3Int(coords.x, coords.y, 0));
    }

    public Vector2Int WorldCoordsToGridCoords(Vector3 coords)
    {
        Vector3Int coordinates = grid.WorldToCell(coords);
        return new Vector2Int(coordinates.x, coordinates.y);
    }


}