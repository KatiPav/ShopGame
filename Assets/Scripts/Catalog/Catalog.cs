
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Catalog
{
    public static Catalog Instance { get; } = new Catalog();

    public Action<InventoryObject> onInventoryObjectAdded;

    //the key is the Prefab ID
    Dictionary<int, InventoryObject> allObjects = new Dictionary<int, InventoryObject>();

    //objects need to be easily accesible by categories so we keep track of the category and its objects(by PrefabId)
    Dictionary<Category, HashSet<int>> categoryObjects = new Dictionary<Category, HashSet<int>>();

    private Catalog()
    {
        foreach (Category c in Enum.GetValues(typeof(Category)))
        {
            categoryObjects.Add(c, new HashSet<int>());
        }
    }
    public InventoryObject GetInventoryObject(int prefabId)
    {
        if (allObjects.ContainsKey(prefabId))
        {
            return allObjects[prefabId];
        }
        else return null;

    }

    public void Add(InventoryObject obj)
    {
        if (allObjects.ContainsKey(obj.PrefabId))
        {
            allObjects[obj.PrefabId] = obj;
            //no need to update the categories as they are already set
            return;
        }

        allObjects.Add(obj.PrefabId, obj);
        foreach (Category c in obj.Categories)
        {
            categoryObjects[c].Add(obj.PrefabId);
        }



    }

    public void Remove(InventoryObject iObj)
    {
        allObjects.Remove(iObj.PrefabId);
        foreach (Category c in iObj.Categories)
        {
            categoryObjects[c].Remove(iObj.PrefabId);
        }
    }

    public List<InventoryObject> GetAllObjects()
    {
        return allObjects.Values.ToList();
    }

    public List<InventoryObject> GetObjectsOfCategory(Category category)
    {
        List<InventoryObject> result = new List<InventoryObject>();
        foreach (int prefabId in categoryObjects[category])
        {
            result.Add(allObjects[prefabId]);
        }
        Debug.Log("We found " + result.Count + "objects of category " + category.ToString());
        return result;
    }

}