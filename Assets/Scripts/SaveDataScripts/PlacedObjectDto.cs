using UnityEngine;

public class PlacedObjectDto : IObjectDto
{
    public int x;
    public int y;
    public int PrefabId { get; set; }

    public PlacedObjectDto(Vector2Int coords, int prefabId)
    {
        x = coords.x;
        y = coords.y;
        this.PrefabId = prefabId;
    }
}

