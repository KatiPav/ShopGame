using UnityEngine;

public class GridObjectFactory : MonoBehaviour
{
    [SerializeField]
    ObjectDatabase objectDatabase;

    [SerializeField]
    GridConverter gridCoordinates;

    public Item CreateGridItem(PlacedObjectDto obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        prefab.SetActive(true);
        Vector3 position = gridCoordinates.GridCoordsToWorldCoords(new Vector2Int(obj.x, obj.y));

        GameObject newObj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        Item item = ConfigureItemComponent(newObj, obj);
        return item;
    }

    private Item ConfigureItemComponent(GameObject itemObj, PlacedObjectDto obj)
    {
        Item item = itemObj.GetComponent<Item>();

        if (item == null)
        {
            item = itemObj.AddComponent<Item>();
        }

        item.Initialize(obj.PrefabId, obj.ItemType, new Vector2Int(obj.x, obj.y));
        return item;
    }
}