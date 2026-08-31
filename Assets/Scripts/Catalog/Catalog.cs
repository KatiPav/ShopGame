


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Catalog
{
    public static Catalog Instance { get; } = new Catalog();

    public Action<InventoryObject> onInventoryObjectAdded;
    Dictionary<string, InventoryObject> allObjects = new Dictionary<string, InventoryObject>();

    Dictionary<Category, HashSet<string>> categoryObjects = new Dictionary<Category, HashSet<string>>();

    private Catalog()
    {
        foreach (Category c in Enum.GetValues(typeof(Category)))
        {
            categoryObjects.Add(c, new HashSet<string>());
        }
    }

    public void Add(InventoryObject obj)
    {
        allObjects.Add(obj.Id, obj);
        foreach (var category in obj.Categories)
        {
            categoryObjects[category].Add(obj.Id);
        }
        onInventoryObjectAdded.Invoke(obj);
    }

    public List<InventoryObject> GetAllObjects()
    {
        return allObjects.Values.ToList();
    }

    public List<InventoryObject> GetObjectsOfCategory(Category category)
    {
        List<InventoryObject> result = new List<InventoryObject>();
        foreach (string id in categoryObjects[category])
        {
            result.Add(allObjects[id]);
        }
        return result;
    }

    public InventoryObject Remove(InventoryObject objToRemove)
    {
        foreach (var category in objToRemove.Categories)
        {
            categoryObjects[category].Remove(objToRemove.Id);
        }
        allObjects.Remove(objToRemove.Id);

        return objToRemove;

    }

}