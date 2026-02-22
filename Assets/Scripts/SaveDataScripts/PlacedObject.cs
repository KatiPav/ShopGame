using UnityEngine;

public class PlacedObject
{
    public int x;
    public int y;
    public int id;

    public PlacedObject(Vector2Int coords, int id)
    {
        x = coords.x;
        y = coords.y;
        this.id = id;
    }
}

