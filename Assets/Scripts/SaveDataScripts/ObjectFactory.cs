using UnityEngine;
using System;
using System.Collections.Generic;

public class GameItemFactory : MonoBehaviour
{
    [SerializeField]
    ObjectDatabase objectDatabase;

    [SerializeField]
    GridConverter gridCoordinates;

    [SerializeField]
    private ItemRuntimeSet itemRuntimeSet;

    public Action<Item> onItemCreated;

    public Item CreateGridItem(PlacedObjectDto obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        Vector3 position = gridCoordinates.GridCoordsToWorldCoords(new Vector2Int(obj.x, obj.y));

        GameObject newObj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Item item = AttachItemComponent(newObj);
        newObj.SetActive(true);
        InitializeItem(item, obj.PrefabId, new Vector2Int(obj.x, obj.y));
        onItemCreated?.Invoke(item);
        return item;
    }

    public InventoryObject CreateInventoryObject(InventoryObjectDto obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        return new InventoryObject(obj.Id, obj.PrefabId, prefab.GetComponent<SpriteRenderer>().sprite, obj.amount, obj.Categories);
    }

    public InventoryObject CreateInventoryObject(int prefabId, List<Category> categories)
    {
        GameObject prefab = objectDatabase.GetPrefabById(prefabId);
        return new InventoryObject(Guid.NewGuid(), prefabId, prefab.GetComponent<SpriteRenderer>().sprite, 1, categories);
    }

    public Item CreateGridItem(InventoryObject obj) //this is the same as the other function, check why and how to not duplicate
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        Vector3 position = Input.mousePosition;

        GameObject newObj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Item item = AttachItemComponent(newObj);

        newObj.SetActive(true);
        InitializeItem(item, obj.PrefabId, default);
        onItemCreated?.Invoke(item);
        Debug.Log("create grid item from inventory object called");
        return item;
    }

    private Item AttachItemComponent(GameObject itemObj)
    {
        Item item = itemObj.GetComponent<Item>();

        if (item == null)
        {
            item = itemObj.AddComponent<Item>();
        }

        return item;
    }

    private void InitializeItem(Item item, int prefabId, Vector2Int coords)
    {
        item.Initialize(prefabId, coords, itemRuntimeSet);
    }
}