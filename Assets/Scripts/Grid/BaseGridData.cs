using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BaseGridData
{

    Dictionary<Vector2Int, Guid> cellIdDictionary;
    Dictionary<Guid, GameObject> placedItems;

    public BaseGridData()
    {
        cellIdDictionary = new Dictionary<Vector2Int, Guid>();
        placedItems = new Dictionary<Guid, GameObject>();
    }

    public bool HasPlacedItem(Vector2Int coords)
    {
        return cellIdDictionary.ContainsKey(coords);
    }

    public List<GameObject> getItems()
    {
        return placedItems.Select((a) => { return a.Value; }).ToList();
    }

    public bool TryAddItem(GameObject itemObj)
    {
        Item item = itemObj.GetComponent<Item>();
        if (placedItems.ContainsKey(item.Id))
        {
            Debug.Log("Item already in dictionary 2");
            return false;
        }

        foreach (Vector2Int cell in item.getFloorCells())
        {
            if (cellIdDictionary.ContainsKey(cell))
            {
                Debug.Log("Item already in dictionary 3");
                return false;
            }
        }

        AddFloorCells(item);
        placedItems.Add(item.Id, itemObj);
        return true;
    }

    public bool TryPullItem(GameObject gObj)
    {

        Item item = gObj.GetComponent<Item>();
        if (!placedItems.ContainsKey(item.Id))
        {
            Debug.Log("item not in dictionary");
            return false;
        }
        RemoveFloorCells(item);
        placedItems.Remove(item.Id);
        return true;
    }

    private void AddFloorCells(Item item)
    {
        foreach (Vector2Int cell in item.getFloorCells())
        {
            cellIdDictionary.Add(cell, item.Id);
        }
    }
    private void RemoveFloorCells(Item item)
    {
        foreach (Vector2Int cell in item.getFloorCells())
        {
            cellIdDictionary.Remove(cell);
        }
    }
}
