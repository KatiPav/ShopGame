using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class StorageMenu : SideMenu
{

    [SerializeField]
    CategoryInventory CategoryInventoryPrefab;

    Dictionary<int, CategoryInventory> categoryMap;

    [SerializeField]
    Inventory inventory;

    public override void Awake()
    {
        base.Awake();
        categoryMap = transform.GetComponentsInChildren<CategoryInventory>().ToDictionary(inv => inv.gameObject.layer, inv => inv);
        if (categoryMap.Count == 0)
        {
            Debug.Log("There are no categories in the inventory!");
        }

        if (inventory == null)
        {
            Debug.Log("Inventory not assigned. Did you forget to reference it in storage menu?");
        }
        inventory.OnItemAdded += AddToInventory;
    }

    public void AddToInventory(GameObject item, int category)
    {
        if (categoryMap.TryGetValue(category, out CategoryInventory categoryInventory))
        {
            categoryInventory.Add(item);
        }
        else
        {
            Debug.Log("Could not find the category for item.");
        }
    }
}