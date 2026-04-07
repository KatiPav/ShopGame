using UnityEngine;
using System.Collections.Generic;
using System;

public class GridRegistry
{
    FurnitureGridData furnitureGridData = new FurnitureGridData();
    DecorationsGridData decorationsGridData = new DecorationsGridData();

    public GameObject PullItem(GameObject gObj)
    {
        if (decorationsGridData.TryPullItem(gObj) || furnitureGridData.TryPullItem(gObj))
        {
            return gObj;
        }
        Debug.Log("object not in grids!");
        return null;
    }

    public void AddItem(GameObject item)
    {
        if (!TryAddToAppropriateGrid(item))
        {
            Debug.Log("Could not add object");
            return;
        }
    }


    private bool TryAddToAppropriateGrid(GameObject item)
    {
        switch (item.layer)
        {
            case 7://rename these to understandable constants or enums
                return furnitureGridData.TryAddItem(item);
            case 6:
                return decorationsGridData.TryAddItem(item);
        }
        return false;
    }

    public bool HasPlacedItem(Vector2Int coords)
    {
        return furnitureGridData.HasPlacedItem(coords) || decorationsGridData.HasPlacedItem(coords);
    }

    public List<GameObject> GetFurniture()
    {
        return furnitureGridData.getItems();
    }

    public List<GameObject> GetDecorations()
    {
        return decorationsGridData.getItems();
    }
}