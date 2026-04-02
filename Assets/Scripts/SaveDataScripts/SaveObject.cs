using UnityEngine;

public class SaveObject
{
    public int x;
    public int y;
    public int prefabId;

    public SaveObject(Vector2Int coords, int prefabId)
    {
        x = coords.x;
        y = coords.y;
        this.prefabId = prefabId;
    }
}

