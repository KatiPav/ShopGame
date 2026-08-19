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
        //Debug.Log(itemToAdd.GetType());
        if (!items.Contains(itemToAdd))
            items.Add(itemToAdd);
        else
        {
            Debug.Log("it seems we are doing this twice");
        }
    }

    public void Remove(Item itemToRemove)
    {
        if (items.Contains(itemToRemove))
            items.Remove(itemToRemove);
    }
}