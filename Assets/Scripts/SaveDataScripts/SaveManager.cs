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

    [SerializeField]
    CatalogController catalogController;

    public void Awake()
    {
        if (objectDatabase == null)
        {
            Debug.Log("ObjectDatabase is not assigned. Did you forget to reference it in SaveManager?");
        }
        if (gridRegistry == null)
        {
            Debug.Log("GridRegistry is not assigned. Did you forget to reference it in SaveManager?");
        }
        if (factory == null)
        {
            Debug.Log("GameItemFactory is not assigned. Did you forget to reference it in SaveManager?");
        }
        if (catalogController == null)
        {
            Debug.Log("CatalogController is not assigned. Did you forget to reference it in SaveManager?");
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
            InventoryObject iObj = factory.CreateInventoryObject(obj);
            catalogController.AddNewInventoryObjectToCatalog(iObj);
        }

        InventoryObjectDto test1 = MakeTestInventoryObject(6, 3);
        InventoryObjectDto test2 = MakeTestInventoryObject(1, 3);
        InventoryObjectDto test3 = MakeTestInventoryObject(2, 3);
        InventoryObjectDto test4 = MakeTestInventoryObject(4, 3);
        InventoryObjectDto test5 = MakeTestInventoryObject(3, 3);
        // InventoryObjectDto test2 = MakeTestInventoryObject(4, Category.Decoration, 1);
        // InventoryObjectDto test3 = MakeTestInventoryObject(0, Category.WallDecoration, 2);

        InventoryObject itemtest = factory.CreateInventoryObject(test1);
        InventoryObject itemtest2 = factory.CreateInventoryObject(test2);
        InventoryObject itemtest3 = factory.CreateInventoryObject(test3);
        InventoryObject itemtest4 = factory.CreateInventoryObject(test4);
        InventoryObject itemtest5 = factory.CreateInventoryObject(test5);

        catalogController.AddNewInventoryObjectToCatalog(itemtest);
        catalogController.AddNewInventoryObjectToCatalog(itemtest2);
        //catalogController.AddNewInventoryObjectToCatalog(itemtest3);
        //catalogController.AddNewInventoryObjectToCatalog(itemtest4);
        //catalogController.AddNewInventoryObjectToCatalog(itemtest5);
        //Debug.Log("added 2 test object to catalog");

    }

    private InventoryObjectDto MakeTestInventoryObject(int prefabId, int amount)
    {
        return new InventoryObjectDto(Guid.NewGuid(), prefabId, amount);
    }

    PlacedObjectDto PlacedObjectToPlacedObjectDto(GameObject itemObj)
    {
        Item item = itemObj.GetComponent<Item>();
        return new PlacedObjectDto(item.GridCoordinates, item.PrefabId, item.planeCategory);
    }

    InventoryObjectDto InventoryObjectToInventoryObjectDto(InventoryObject iObj)
    {
        return new InventoryObjectDto(iObj.Id, iObj.PrefabId, iObj.Amount);
    }

    public void SaveGame()
    {
        List<GameObject> gridObjects = gridRegistry.GetAllObjects();
        List<PlacedObjectDto> placedObjectsDtos = gridObjects.Select((item) => { return PlacedObjectToPlacedObjectDto(item); }).ToList();
        List<InventoryObject> inventoryObjects = catalogController.GetAllInventoryObjects();
        List<InventoryObjectDto> inventoryObjectsDtos = inventoryObjects.Select((item) => InventoryObjectToInventoryObjectDto(item)).ToList();

        saveData.Clear();
        saveData.AddObjects(placedObjectsDtos);
        saveData.AddObjects(inventoryObjectsDtos);
        saveData.Save();
    }
}