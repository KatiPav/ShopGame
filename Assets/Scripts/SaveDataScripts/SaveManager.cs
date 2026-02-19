using UnityEngine;

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

        //testing only
        PlacedObject test1 = new PlacedObject();
        test1.coordinates.x = 2;
        test1.coordinates.y = 3;
        test1.id = 0;

        GameObject testItem = PlacedObjectToGameObject(test1);
        placementManager.LoadObjectInGame(testItem, test1.coordinates);

        PlacedObject test2 = new PlacedObject();
        test2.coordinates.x = 4;
        test2.coordinates.y = 6;
        test2.id = 1;
        GameObject testItem2 = PlacedObjectToGameObject(test2);
        placementManager.LoadObjectInGame(testItem2, test2.coordinates);

        //for ech obj in savedata instantiate GameObject and add it to appropriate layer
        foreach (PlacedObject obj in saveData.GetPlacedObjects())
        {
            GameObject item = PlacedObjectToGameObject(obj);
            placementManager.LoadObjectInGame(item, obj.coordinates);
        }
    }

    GameObject PlacedObjectToGameObject(PlacedObject placedObject)
    {
        GameObject prefab = objectDatabase.GetPrefabById(placedObject.id);
        return Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    void GameObjectToPlacedObject()
    {

    }

    public void SaveGame()
    {
        saveData.Save();
        //write all objects to SaveData
        //do we call SaveData.writeToJson here??
    }
}