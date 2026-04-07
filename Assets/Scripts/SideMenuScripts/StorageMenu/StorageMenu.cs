using UnityEngine;
using System.Collections.Generic;
public class StorageMenu : SideMenu
{
    List<GameObject> boughtItems;
    public void Start()
    {

    }

    private void InstantiateStoredItems()
    {
        foreach (GameObject item in boughtItems)
        {
            Vector3 newPosition = new Vector3(0, 0, 0);
            Instantiate(item, newPosition, Quaternion.identity, this.transform);
        }
    }

}