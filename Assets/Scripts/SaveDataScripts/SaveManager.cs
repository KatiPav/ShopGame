using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    SaveData saveData;

    [SerializeField]
    ObjectDatabase objectDatabase;

    [SerializeField]
    PlacementManager placementManager;

    public void Awake()
    {
        if (objectDatabase == null)
        {
            Debug.Log("ObjectDatabase is not assigned. Did you forget to reference it in SaveManager?");
        }

        if (placementManager == null)
        {
            Debug.Log("PlacementManager is not assigned. Did you forget to reference it in SaveManager?");
        }

        saveData = new SaveData();
        LoadObjectsInGame();
    }

    void LoadObjectsInGame()
    {
        foreach (PlacedObject obj in saveData.GetPlacedObjects())
        {
            GameObject item = PlacedObjectToGameObject(obj);
            placementManager.LoadObjectInGame(item, new Vector2Int(obj.x, obj.y));
        }
    }

    GameObject PlacedObjectToGameObject(PlacedObject placedObject)
    {
        GameObject prefab = objectDatabase.GetPrefabById(placedObject.id);

        GameObject item = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Id idComp = item.AddComponent<Id>();
        idComp.id = placedObject.id;
        return item;
    }

    PlacedObject GameObjectToPlacedObject(GameObject item, Vector2Int coords)
    {
        return new PlacedObject(coords, item.GetComponent<Id>().id);
    }

    public void SaveGame()
    {
        List<PlacedObject> itemList = new List<PlacedObject>();
        //get all newly placed objects and add them to savedata and then save
        Dictionary<Vector2Int, GameObject> furniture = placementManager.GetFurnitureGridDataPlacedObjects();
        foreach ((Vector2Int coords, GameObject item) in furniture)
        {
            itemList.Add(GameObjectToPlacedObject(item, coords));
        }


        Dictionary<Vector2Int, GameObject> decorations = placementManager.GetDecorationsGridDataPlacedObjects();

        foreach ((Vector2Int coords, GameObject item) in decorations)
        {
            itemList.Add(GameObjectToPlacedObject(item, coords));
        }
        saveData.UpdateList(itemList);
        saveData.Save();
    }
}