using UnityEngine;
using System;

public class GridObjectFactory : MonoBehaviour
{
    [SerializeField]
    ObjectDatabase objectDatabase;

    [SerializeField]
    GridConverter gridCoordinates;

    public Action<Item> onItemCreated;

    public Item CreateGridItem(PlacedObjectDto obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        prefab.SetActive(true);
        Vector3 position = gridCoordinates.GridCoordsToWorldCoords(new Vector2Int(obj.x, obj.y));

        GameObject newObj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        //AttachRigidBody2D(newObj);
        Item item = AttachItemComponent(newObj, obj);

        onItemCreated?.Invoke(item);
        return item;
    }

    private Item AttachItemComponent(GameObject itemObj, PlacedObjectDto obj)
    {
        Item item = itemObj.GetComponent<Item>();

        if (item == null)
        {
            item = itemObj.AddComponent<Item>();
        }

        item.Initialize(obj.PrefabId, new Vector2Int(obj.x, obj.y));
        return item;
    }

    private Rigidbody2D AttachRigidBody2D(GameObject gObj)
    {
        Rigidbody2D rb = gObj.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gObj.AddComponent<Rigidbody2D>();
        }

        Debug.Log("huh?");
        rb.bodyType = RigidbodyType2D.Kinematic;

        return rb;
    }
}