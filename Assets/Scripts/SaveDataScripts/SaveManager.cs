using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

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
    }

    public void LoadObjectsInGame()
    {
        foreach (SaveObject obj in saveData.GetSaveObjects())
        {
            GameObject prefab = objectDatabase.GetPrefabById(obj.prefabId);
            GameObject itemObj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            Item item = itemObj.AddComponent<Item>();
            item.GridCoordinates = new Vector2Int(0, 0);
            item.PrefabId = obj.prefabId;
            item.FloorShape = prefab.GetComponent<FloorShape>().shapeCells;
            placementManager.PlaceItemObjectAt(itemObj, new Vector2Int(obj.x, obj.y));
        }
    }

    SaveObject ItemToSaveObject(GameObject itemObj)
    {
        Item item = itemObj.GetComponent<Item>();
        return new SaveObject(item.GridCoordinates, item.PrefabId);
    }

    public void SaveGame()
    {
        List<SaveObject> saveObjects = new List<SaveObject>();

        List<GameObject> furniture = placementManager.gridRegistry.GetFurniture();
        List<GameObject> decorations = placementManager.gridRegistry.GetDecorations();

        List<GameObject> items = furniture.Concat(decorations).ToList();

        saveObjects = items.Select((item) => { return ItemToSaveObject(item); }).ToList();

        saveData.UpdateList(saveObjects);
        saveData.Save();
    }
}