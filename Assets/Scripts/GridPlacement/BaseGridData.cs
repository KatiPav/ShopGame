using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseGridData
{

    Dictionary<Vector2Int, GameObject> placedObjects;
    public BaseGridData()
    {
        placedObjects = new Dictionary<Vector2Int, GameObject>();
    }

    public Dictionary<Vector2Int, GameObject> GetPlacedObjects()
    {
        return placedObjects;
    }

    public void TryPlaceObject(GameObject gObj, Vector2Int coordinates)
    {
        Vector2Int key = new Vector2Int(coordinates.x, coordinates.y);

        if (!placedObjects.TryAdd(key, gObj))
        {
            ThrowDuplicateKeyError();

        }

        Debug.Log("obj " + gObj + " added at " + coordinates);
    }

    protected virtual void ThrowDuplicateKeyError()
    {
        Debug.Log("There is something here already. Move it first.");//change this to user messages 
    }

    public void GetObjectAt(int x, int y)
    {

    }

    public bool TryPickUpObject(Vector2Int coordinates, out GameObject obj)
    {
        Vector2Int key = new Vector2Int(coordinates.x, coordinates.y);
        if (!placedObjects.ContainsKey(key))
        {
            obj = null;
            return false;
        }
        obj = placedObjects[key];
        placedObjects.Remove(key);

        return true;
    }
}
