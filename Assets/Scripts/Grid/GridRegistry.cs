using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class GridRegistry : MonoBehaviour
{
    BaseGridData floorGridData = new BaseGridData();
    BaseGridData leftWallGridData = new BaseGridData();
    BaseGridData rightWallGridData = new BaseGridData();

    public Item RemoveItem(Item item)
    {
        BaseGridData grid = GetGridByCategory(item.planeCategory);

        if (grid.RemoveItem(item))
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
        BaseGridData grid = GetGridByCategory(item.planeCategory);

        return grid.TryAddItem(item);
    }

    public bool CanPlaceItemAt(Vector2Int coords, Item item)
    {
        BaseGridData grid = GetGridByCategory(item.planeCategory);

        foreach (Vector2Int cell in item.shape.GetCellsWithOrigin(coords))
        {
            if (grid.HasPlacedItem(cell)) return false;
        }
        return true;
    }

    private BaseGridData GetGridByCategory(PlaneCategory cat)
    {
        switch (cat)
        {
            case PlaneCategory.Floor:
                return floorGridData;
            case PlaneCategory.LeftWall:
                return leftWallGridData;
            case PlaneCategory.RightWall:
                return rightWallGridData;
        }
        Debug.Log("Something is wrong, item has a weird type and registry is confused!");
        return floorGridData;
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

        outCoords = coords;
        foreach (Vector2Int offset in offsets)
        {
            Vector2Int newCoords = coords + offset;

            if (CanPlaceItemAt(newCoords, item))
            {
                outCoords = newCoords;
                return true;
            }
        }
        return false;
    }

    public List<GameObject> GetAllObjects()
    {
        List<GameObject> floor = floorGridData.getItems();
        List<GameObject> leftwall = leftWallGridData.getItems();
        return floor.Concat(leftwall).ToList();
    }
}