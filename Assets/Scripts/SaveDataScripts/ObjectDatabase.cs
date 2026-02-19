using UnityEngine;
using System.Collections.Generic;


//copied this from AI
[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "Database/ObjectDatabase")]
public class ObjectDatabase : ScriptableObject
{
    public static ObjectDatabase Instance;

    public List<PrefabId> objects;
    Dictionary<int, GameObject> lookup = null;

    public GameObject GetPrefabById(int id)
    {
        // i dont know if this is the correct approach, but i dont have better. 
        // I cant use awake to init as it is called too late and I dont think OnEnable is a good idea as it might be called too often.
        if (lookup == null)
        {
            lookup = new Dictionary<int, GameObject>();
            foreach (PrefabId obj in objects)
                lookup[obj.id] = obj.prefab;
        }

        if (lookup.TryGetValue(id, out GameObject prefab))
            return prefab;

        Debug.LogError("No prefab found for id " + id);
        return null;
    }
}