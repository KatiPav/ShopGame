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
    private ItemRuntimeSet floorItems;

    [SerializeField]
    private ItemRuntimeSet leftWallItems;

    [SerializeField]
    private ItemRuntimeSet rightWallItems;

    public Action<Item> onItemCreated;

    public Item CreateGridItem(PlacedObjectDto obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        Vector3 position = gridCoordinates.GridCoordsToWorldCoords(new Vector2Int(obj.X, obj.Y), obj.PlaneCat);

        GameObject newObj = Instantiate(prefab, position, Quaternion.identity);

        Item item = GetOrAttachItemComponent(newObj);
        newObj.SetActive(true);
        item.Initialize(obj.PrefabId, new Vector2Int(obj.X, obj.Y), GetRuntimeSet(item.planeCategory), item.catalogCategory, item.planeCategory);

        onItemCreated?.Invoke(item);
        return item;
    }

    public InventoryObject CreateInventoryObject(InventoryObjectDto obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        Item item = GetOrAttachItemComponent(prefab);
        return new InventoryObject(obj.Id, obj.PrefabId, prefab.GetComponent<SpriteRenderer>().sprite, obj.Amount, item.catalogCategory);
    }

    public InventoryObject CreateInventoryObject(int prefabId, CatalogCategory cat)
    {
        GameObject prefab = objectDatabase.GetPrefabById(prefabId);
        return new InventoryObject(Guid.NewGuid(), prefabId, prefab.GetComponent<SpriteRenderer>().sprite, 1, cat);
    }

    public Item CreateGridItem(InventoryObject obj) //this is the same as the other function, check why and how to not duplicate
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        Vector3 position = Input.mousePosition;
        //ItemRuntimeSet set = GetRuntimeSet(obj.Ca)


        GameObject newObj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Item item = GetOrAttachItemComponent(newObj);
        Debug.Log("item has catalog category " + item.catalogCategory);

        newObj.SetActive(true);
        item.Initialize(obj.PrefabId, default, GetRuntimeSet(item.planeCategory), item.catalogCategory, item.planeCategory);

        onItemCreated?.Invoke(item);
        return item;
    }

    private ItemRuntimeSet GetRuntimeSet(PlaneCategory cat)
    {
        if (cat == PlaneCategory.Floor) return floorItems;
        if (cat == PlaneCategory.LeftWall) return leftWallItems;
        if (cat == PlaneCategory.RightWall) return rightWallItems;
        Debug.Log("Cannot find Runtime Set for this item type!");
        return floorItems;
    }

    private Item GetOrAttachItemComponent(GameObject itemObj)
    {
        Item item = itemObj.GetComponent<Item>();

        if (item == null)
        {
            item = itemObj.AddComponent<Item>();
            Debug.Log("Item component not found, attahing a default one!");
        }

        return item;
    }
}