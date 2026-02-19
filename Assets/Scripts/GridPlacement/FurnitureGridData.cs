using UnityEngine;

public class FurnitureGridData : BaseGridData
{

    protected override void ThrowDuplicateKeyError()
    {
        Debug.Log("There is furniture here already. Try placing some items on it.");//change this to user messages 
    }

}