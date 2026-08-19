using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Runtime Set")]
public class ItemRuntimeSet : ScriptableObject
{
    [SerializeField]
    private List<Item> items = new List<Item>();
    public List<Item> Items => items;
    private void OnEnable()//just in case
    {
        items.Clear();
    }
    public void Add(Item itemToAdd)
    {
        if (!items.Contains(itemToAdd))
            items.Add(itemToAdd);
    }

    public void Remove(Item itemToRemove)
    {
        if (items.Contains(itemToRemove))
            items.Remove(itemToRemove);
    }
}