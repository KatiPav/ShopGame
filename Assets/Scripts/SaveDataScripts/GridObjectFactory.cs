using UnityEngine;

public class GridObjectFactory : MonoBehaviour
{
    [SerializeField]
    ObjectDatabase objectDatabase;

    [SerializeField]
    GridCoordinates gridCoordinates;

    public GameObject CreateGridObject(PlacedObjectDto obj)
    {
        GameObject prefab = objectDatabase.GetPrefabById(obj.PrefabId);
        prefab.SetActive(true);
        Vector3 position = gridCoordinates.GridCoordsToWorldCoords(new Vector2Int(obj.x, obj.y));
        GameObject newObj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        AddItemComponent(newObj, obj);
        return newObj;
    }

    private void AddItemComponent(GameObject itemObj, PlacedObjectDto obj)
    {
        Item item = itemObj.GetComponent<Item>();

        if (item == null)
        {
            item = itemObj.AddComponent<Item>();
        }

        var floorShape = itemObj.GetComponent<FloorShape>();

        if (floorShape == null)
        {
            Debug.LogError("Missing FloorShape!");
            return;
        }

        item.Initialize(obj.PrefabId, floorShape.shapeCells, new Vector2Int(obj.x, obj.y));
    }
}