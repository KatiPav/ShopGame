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
    GridRegistry gridRegistry;

    [SerializeField]
    GridObjectFactory factory;

    public void Awake()
    {
        if (objectDatabase == null)
        {
            Debug.Log("ObjectDatabase is not assigned. Did you forget to reference it in SaveManager?");
        }

        saveData = new SaveData();
        LoadSavedObjectsIntoRegistry();
    }

    private void LoadSavedObjectsIntoRegistry()
    {
        foreach (PlacedObjectDto obj in saveData.saveObjects.placedObjects)
        {
            Item item = factory.CreateGridItem(obj);
            gridRegistry.AddItem(item);
        }
    }

    PlacedObjectDto PlacedObjectToPlacedObjectDto(GameObject itemObj)
    {
        Item item = itemObj.GetComponent<Item>();
        return new PlacedObjectDto(item.GridCoordinates, item.PrefabId, item.ItemType);
    }

    public void SaveGame()
    {
        List<GameObject> gridObjects = gridRegistry.GetAllObjects();
        List<PlacedObjectDto> placedObjectsDtos = gridObjects.Select((item) => { return PlacedObjectToPlacedObjectDto(item); }).ToList();

        Debug.Log("save manager reached");
        saveData.Clear();
        saveData.AddObjects(placedObjectsDtos);
        saveData.Save();
    }
}