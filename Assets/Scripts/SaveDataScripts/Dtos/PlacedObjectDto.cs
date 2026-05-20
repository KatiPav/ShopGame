using UnityEngine;

public class PlacedObjectDto : IObjectDto
{
    public int x;
    public int y;
    public int PrefabId { get; set; }

    public ItemType ItemType { get; set; }

    public PlacedObjectDto(Vector2Int coords, int prefabId, ItemType itemType)
    {
        x = coords.x;
        y = coords.y;
        PrefabId = prefabId;
        ItemType = itemType;
    }
}

