using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class GridRegistry : MonoBehaviour
{
    [SerializeField]
    Grid gridCoordinates;
    FurnitureGridData furnitureGridData = new FurnitureGridData();
    DecorationsGridData decorationsGridData = new DecorationsGridData();

    public Item PullItem(Item item)
    {
        if (decorationsGridData.TryPullItem(item) || furnitureGridData.TryPullItem(item))
        {
            return item;
        }
        Debug.Log("object not in grids!");
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
            case ItemType.Furniture:
                return furnitureGridData.TryAddItem(item);
            case ItemType.Decoration:
                return decorationsGridData.TryAddItem(item);
        }
        return false;
    }

    public bool CanPlaceItemAt(Vector2Int coords)
    {
        return !furnitureGridData.HasPlacedItem(coords) && !decorationsGridData.HasPlacedItem(coords);
    }

    public List<GameObject> GetFurniture()
    {
        return furnitureGridData.getItems();
    }

    public List<GameObject> GetDecorations()
    {
        return decorationsGridData.getItems();
    }

    public List<GameObject> GetAllObjects()
    {
        List<GameObject> furniture = furnitureGridData.getItems();
        List<GameObject> decorations = decorationsGridData.getItems();
        return furniture.Concat(decorations).ToList();
    }
}