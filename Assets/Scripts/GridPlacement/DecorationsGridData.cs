using UnityEngine;

public class DecorationsGridData : BaseGridData
{
    protected override void ThrowDuplicateKeyError()
    {
        Debug.Log("There is an item here already. Try moving it first.");//change this to user messages 
    }
}