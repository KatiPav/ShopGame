using UnityEngine;
using System.Collections.Generic;
using System;
public class Inventory : MonoBehaviour
{
    List<GameObject> furniture;
    List<GameObject> decorations;

    public event Action<GameObject, int> OnItemAdded;
    public void Awake()
    {
        furniture = new List<GameObject>();
        decorations = new List<GameObject>();
    }
    public List<GameObject> GetFurniture()
    {
        return furniture;
    }

    public List<GameObject> GetDecorations()
    {
        return decorations;
    }

    public void Add(GameObject item)
    {
        switch (item.layer)
        {
            case 7://rename these to understandable constants or enums
                furniture.Add(item);
                OnItemAdded.Invoke(item, item.layer);
                Debug.Log("obj was added to furniture inventory");
                break;
            case 6:
                decorations.Add(item);
                OnItemAdded.Invoke(item, item.layer);
                Debug.Log("obj was added to decorations inventory");
                break;
        }
    }
}