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

    public List<GameObject> getItems()
    {
        return placedItems.Select((a) => { return a.Value; }).ToList();
    }

    public void AddItem(GameObject itemObj)
    {
        Item item = itemObj.GetComponent<Item>();

        Debug.Log("adding " + item.PrefabId + " at " + item.GridCoordinates);
        foreach (Vector2Int cell in item.getFloorCells())
        {
            cellIdDictionary.Add(cell, item.Id);
        }
        placedItems.Add(item.Id, itemObj);


        Debug.Log("cellId is ");
        Debug.Log(string.Join(", ", cellIdDictionary.Keys));
    }

    protected virtual void ThrowDuplicateKeyError()
    {
        Debug.Log("There is something here already. Move it first.");//change this to user messages 
    }

    public GameObject PullItem(Vector2Int coordinates)
    {
        if (!cellIdDictionary.ContainsKey(coordinates))
        {
            Debug.Log("could not remove object");
            return null;
        }


        Guid id = cellIdDictionary[coordinates];
        GameObject itemObj = placedItems[id];
        Item item = itemObj.GetComponent<Item>();
        foreach (Vector2Int cell in item.getFloorCells())
        {
            cellIdDictionary.Remove(cell);
        }
        placedItems.Remove(id);

        Debug.Log("cellId is ");
        Debug.Log(string.Join(", ", cellIdDictionary.Keys));

        return itemObj;
    }

}
