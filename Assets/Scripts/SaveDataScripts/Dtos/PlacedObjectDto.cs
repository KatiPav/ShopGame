using UnityEngine;

public class PlacedObjectDto : IObjectDto
{
    public int X { get; set; }
    public int Y { get; set; }

    public int PrefabId { get; set; }

    public PlaneCategory PlaneCat { get; set; }

    public PlacedObjectDto(Vector2Int coords, int prefabId, PlaneCategory planeCategory)
    {
        X = coords.x;
        Y = coords.y;
        PrefabId = prefabId;
        PlaneCat = planeCategory;
    }
}

