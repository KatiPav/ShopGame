using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class GridRegistry : MonoBehaviour
{
    [SerializeField]
    Grid gridCoordinates;
    BaseGridData furnitureGridData = new BaseGridData();
    BaseGridData decorationsGridData = new BaseGridData();

    BaseGridData wallDecorationsGridData = new BaseGridData();

    public Item RemoveItem(Item item)
    {

        if (item.ItemType == Category.WallDecoration && wallDecorationsGridData.RemoveItem(item))
        {
            return item;
        }

        if (item.ItemType == Category.Decoration && decorationsGridData.RemoveItem(item))
        {
            return item;
        }
        if (item.ItemType == Category.Furniture && furnitureGridData.RemoveItem(item))
        {
            return item;
        }

        Debug.Log("Failed to remove object from registry! Object not in grids!");
        return null;
    }

    public void AddItem(Item item)
    {
        if (!TryAddToAppropriateGrid(item))
        {
            Debug.Log("Could not add object");
            return;
        }
    }


    private bool TryAddToAppropriateGrid(Item item)
    {
        switch (item.ItemType)
        {
            case Category.Furniture:
                return furnitureGridData.TryAddItem(item);
            case Category.Decoration:
                return decorationsGridData.TryAddItem(item);
            case Category.WallDecoration:
                return wallDecorationsGridData.TryAddItem(item);
        }

        return false;
    }

    public bool CanPlaceItemAt(Vector2Int coords, Item item)
    {

        foreach (Vector2Int cell in item.shape.GetCellsWithOrigin(coords))
        {
            if (furnitureGridData.HasPlacedItem(cell)) return false;
            if (decorationsGridData.HasPlacedItem(cell)) return false;
        }
        return true;
    }
    public bool CanPlaceItemInOrAraoundCoords(Item item, Vector2Int coords, out Vector2Int outCoords)
    {
        Vector2Int[] offsets =
{
            new Vector2Int(0,0),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1)
        };

        bool canPlace = false;
        outCoords = coords;
        foreach (Vector2Int offset in offsets)
        {
            Vector2Int newCoords = coords + offset;

            if (CanPlaceItemAt(newCoords, item))
            {
                canPlace = true;
                outCoords = newCoords;
                break;
            }
        }
        return canPlace;
    }

    public List<GameObject> GetAllObjects()
    {
        List<GameObject> furniture = furnitureGridData.getItems();
        List<GameObject> decorations = decorationsGridData.getItems();
        return furniture.Concat(decorations).ToList();
    }
}