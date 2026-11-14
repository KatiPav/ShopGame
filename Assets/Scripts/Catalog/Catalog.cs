using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Catalog
{
    public static Catalog Instance { get; } = new Catalog();

    // the key is the Prefab ID
    Dictionary<int, InventoryObject> allObjects = new Dictionary<int, InventoryObject>();

    // objects need to be easily accessible by category, so we track each category and its objects (by PrefabId)
    Dictionary<CatalogCategory, HashSet<int>> categoryObjects = new Dictionary<CatalogCategory, HashSet<int>>();

    private Catalog()
    {
        foreach (CatalogCategory c in Enum.GetValues(typeof(CatalogCategory)))
        {
            categoryObjects.Add(c, new HashSet<int>());
        }
    }

    public InventoryObject GetInventoryObject(int prefabId)
    {
        allObjects.TryGetValue(prefabId, out InventoryObject obj);
        return obj;
    }

    public void Add(InventoryObject obj)
    {
        allObjects[obj.PrefabId] = obj;

        Debug.Log("adding object to catalog with category " + obj.CatalogCat);
        categoryObjects[obj.CatalogCat].Add(obj.PrefabId);
    }

    public void Remove(InventoryObject iObj)
    {
        if (allObjects.Remove(iObj.PrefabId))
        {
            categoryObjects[iObj.CatalogCat].Remove(iObj.PrefabId);
        }
    }

    public List<InventoryObject> GetAllObjects()
    {
        return allObjects.Values.ToList();
    }

    public List<InventoryObject> GetObjectsOfCategory(CatalogCategory category)
    {
        List<InventoryObject> result = new List<InventoryObject>();
        foreach (int prefabId in categoryObjects[category])
        {
            result.Add(allObjects[prefabId]);
        }
        return result;
    }
}