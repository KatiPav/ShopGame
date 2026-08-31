using UnityEngine;
using System;

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

    public Item CreateGridItem(InventoryObject obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        Vector3 position = Input.mousePosition;

        GameObject newObj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Item item = AttachItemComponent(newObj);

        newObj.SetActive(true);
        //InitializeItem(item, obj.PrefabId, new Vector2Int(obj.x, obj.y));
        onItemCreated?.Invoke(item);
        //Debug.Log("create grid item called");
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