using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Item : MonoBehaviour
{
    [SerializeField]
    public ItemType ItemType;

    public Guid Id { get; set; }

    public Vector2Int GridCoordinates { get; set; }
    public int PrefabId { get; set; }

    public FloorShape FloorShape { get; set; }

    void Awake()
    {
        Id = Guid.NewGuid();
        FloorShape = GetComponent<FloorShape>();
    }

    public void Initialize(int prefabId, ItemType itemType, Vector2Int coordinates)
    {
        PrefabId = prefabId;
        ItemType = itemType;
        GridCoordinates = coordinates;
    }
    public void MoveTo(Vector2Int coords, GridConverter gridConverter)
    {
        gameObject.transform.position = gridConverter.GridCoordsToWorldCoords(coords);
        GridCoordinates = coords;
    }
}