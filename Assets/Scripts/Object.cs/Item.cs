using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Item : MonoBehaviour
{
    public Guid Id { get; set; }

    public Vector2Int GridCoordinates { get; set; }
    public int PrefabId { get; set; }
    public List<Vector2Int> FloorShape { get; set; }

    void Awake()
    {
        Id = Guid.NewGuid();
        GridCoordinates = new Vector2Int(0, 0);
        FloorShape = new List<Vector2Int>();
    }
    public List<Vector2Int> getFloorCells()
    {
        List<Vector2Int> result = FloorShape.Select(coord => GridCoordinates + coord).ToList<Vector2Int>();
        result.Add(GridCoordinates);
        return result;
    }


}