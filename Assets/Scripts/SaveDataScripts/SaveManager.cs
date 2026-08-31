using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System;

public class SaveManager : MonoBehaviour
{
    SaveData saveData;

    [SerializeField]
    ObjectDatabase objectDatabase;

    [SerializeField]
    GridRegistry gridRegistry;

    [SerializeField]
    GameItemFactory factory;

    public void Awake()
    {
        if (objectDatabase == null)
        {
            Debug.Log("ObjectDatabase is not assigned. Did you forget to reference it in SaveManager?");
        }

        saveData = new SaveData();
        LoadSavedObjectsIntoGridRegistry();
        LoadSavedObjectsIntoCatalog();
    }

    private void LoadSavedObjectsIntoGridRegistry()
    {
        foreach (PlacedObjectDto obj in saveData.saveObjects.placedObjects)
        {
            Item item = factory.CreateGridItem(obj);
            gridRegistry.AddItem(item);
        }
    }

    private void LoadSavedObjectsIntoCatalog()
    {
        foreach (InventoryObjectDto obj in saveData.saveObjects.inventoryObjects)
        {
            InventoryObject item = factory.CreateInventoryObject(obj);
            Catalog.Instance.Add(item); //is it better for catalog to ba an actual object?
        }


        List<Category> cats = new List<Category>();
        cats.Add(Category.Furniture);

        InventoryObjectDto test1 = MakeTestInventoryObject(0, cats, 4);
        InventoryObjectDto test2 = MakeTestInventoryObject(4, cats, 4);

        InventoryObject itemtest = factory.CreateInventoryObject(test1);
        InventoryObject itemtest2 = factory.CreateInventoryObject(test2);
        Catalog.Instance.Add(itemtest); //is it better for catalog to ba an actual object?
        Catalog.Instance.Add(itemtest2); //is it better for catalog to ba an actual object?
        Debug.Log("added 2 test object to catalog");

    }

    private InventoryObjectDto MakeTestInventoryObject(int prefabId, List<Category> cats, int amount)
    {
        InventoryObjectDto test1 = new();
        test1.Id = Guid.NewGuid();
        test1.PrefabId = prefabId;
        test1.Categories = cats;
        test1.amount = amount;
        return test1;
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

        saveData.Clear();
        saveData.AddObjects(placedObjectsDtos);
        saveData.Save();
    }
}