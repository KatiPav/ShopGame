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

    public bool TryAddItem(Item item)
    {
        if (placedItems.ContainsKey(item.Id))
        {
            Debug.Log("Item already in dictionary 2");
            return false;
        }

        foreach (Vector2Int cell in item.FloorShape.GetFloorCells())
        {
            if (cellIdDictionary.ContainsKey(cell))
            {
                Debug.Log("Item already in dictionary 3");
                return false;
            }
        }

        AddFloorCells(item);
        placedItems.Add(item.Id, item.gameObject);
        Debug.Log("added whole item to grid registry with id" + item.Id);
        return true;
    }

    public bool TryPullItem(Item item)
    {
        if (!placedItems.ContainsKey(item.Id))
        {
            Debug.Log("item " + item.Id + " not in dictionary" + GetName());
            Debug.Log("dictionary " + GetName() + " contains ");

            foreach (KeyValuePair<Guid, GameObject> ob in placedItems)
            {
                Debug.Log(ob.Key);
            }
            return false;
        }
        RemoveFloorCells(item);
        placedItems.Remove(item.Id);
        return true;
    }

    private void AddFloorCells(Item item)
    {
        //Debug.Log("trying to add item with origin coords " + item.GridCoordinates.x + ", " + item.GridCoordinates.y);
        foreach (Vector2Int cell in item.FloorShape.GetFloorCells())
        {

            //Debug.Log("adding " + cell.x + "," + cell.y);
            cellIdDictionary.Add(cell, item.Id);
        }
    }
    private void RemoveFloorCells(Item item)
    {
        foreach (Vector2Int cell in item.FloorShape.GetFloorCells())
        {
            cellIdDictionary.Remove(cell);
        }
    }

    protected virtual string GetName()
    {
        return "BaseGridData";
    }
}

