//script to store all objects that were added currently
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    const string SAVEFILE_NAME = "savefile.json";
    public GameObjectsSaveData saveObjects { get; private set; }

    public SaveData()
    {
        saveObjects = new GameObjectsSaveData();
        readJSON();
    }

    public void Save()
    {
        writeJSON();
    }

    void writeJSON()
    {
        //either creates or writes to existing json all the placedObjects
        string json = JsonConvert.SerializeObject(saveObjects);
        File.WriteAllText(SAVEFILE_NAME, json);
        Debug.Log("write json reached");

    }

    public void Clear()
    {
        saveObjects.inventoryObjects.Clear();
        saveObjects.placedObjects.Clear();
    }

    public void AddObjects(List<PlacedObjectDto> objList)
    {
        saveObjects.placedObjects.AddRange(objList);
    }

    public void AddObjects(List<InventoryObjectDto> objList)
    {
        saveObjects.inventoryObjects.AddRange(objList);
    }


    void readJSON()
    {
        //reads from existing json and fills saveObjects
        if (!File.Exists(SAVEFILE_NAME))
        {
            Debug.Log("Savefile not found!");
            return;
        }

        string json = "";
        json = File.ReadAllText(SAVEFILE_NAME);
        saveObjects = JsonConvert.DeserializeObject<GameObjectsSaveData>(json);

        Debug.Log("placed obj" + saveObjects.placedObjects.Count);
    }

}

