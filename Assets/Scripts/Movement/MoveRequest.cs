using UnityEngine;

public class MoveRequest
{
    public Item item;
    public Vector2Int olgGridCoordinates;
    public Vector2Int newGridCoordinates;

    public MoveRequest(Item item, Vector2Int oldCoords, Vector2Int newCoords)
    {
        this.item = item;
        olgGridCoordinates = oldCoords;
        newGridCoordinates = newCoords;
    }
}