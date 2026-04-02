//script to store all objects that were added currently
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    const string SAVEFILE_NAME = "placed_objects.json";
    List<SaveObject> saveObjects;

    public SaveData()
    {
        saveObjects = new List<SaveObject>();
        readJSON();
    }

    public List<SaveObject> GetSaveObjects()
    {
        return saveObjects;
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
    }

    public void UpdateList(List<SaveObject> objList)
    {
        saveObjects = objList;
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
        saveObjects = JsonConvert.DeserializeObject<List<SaveObject>>(json);

        Debug.Log(saveObjects);
    }

}

