using UnityEngine;
using System.Collections.Generic;
using System;
public class Inventory : MonoBehaviour
{
    List<InventoryObject> furniture;
    List<InventoryObject> decorations;

    public event Action<GameObject, int> OnItemAdded;

    public void Awake()
    {
        furniture = new List<InventoryObject>();
        decorations = new List<InventoryObject>();
    }
    public List<InventoryObject> GetFurniture()
    {
        return furniture;
    }

    public List<InventoryObject> GetDecorations()
    {
        return decorations;
    }

    public void Add(InventoryObject item)
    {
        //     switch (item.layer)
        //     {
        //         case 7://rename these to understandable constants or enums
        //             furniture.Add(item);
        //             OnItemAdded.Invoke(item, item.layer);
        //             Debug.Log("obj was added to furniture inventory");
        //             break;
        //         case 6:
        //             decorations.Add(item);
        //             OnItemAdded.Invoke(item, item.layer);
        //             Debug.Log("obj was added to decorations inventory");
        //             break;
        //     }
    }
}