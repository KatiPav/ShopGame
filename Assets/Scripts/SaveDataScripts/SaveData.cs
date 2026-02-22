//script to store all objects that were added currently
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    const string SAVEFILE_NAME = "placed_objects.json";
    List<PlacedObject> placedObjects;

    public SaveData()
    {
        placedObjects = new List<PlacedObject>();
        readJSON();
    }

    public List<PlacedObject> GetPlacedObjects()
    {
        return placedObjects;
    }

    public void Save()
    {
        writeJSON();
    }

    void writeJSON()
    {
        //either creates or writes to existing json all the placedObjects
        string json = JsonConvert.SerializeObject(placedObjects);
        File.WriteAllText(SAVEFILE_NAME, json);
    }

    public void UpdateList(List<PlacedObject> objList)
    {
        placedObjects = objList;
    }

    void readJSON()
    {
        //reads from existing json and fills placedObjects
        if (!File.Exists(SAVEFILE_NAME))
        {
            Debug.Log("Savefile not found!");
            return;
        }

        string json = "";
        json = File.ReadAllText(SAVEFILE_NAME);
        placedObjects = JsonConvert.DeserializeObject<List<PlacedObject>>(json);

        Debug.Log(placedObjects);
    }

}

