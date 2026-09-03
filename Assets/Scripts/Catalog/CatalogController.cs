

using System;
using UnityEngine;

public class CatalogController : MonoBehaviour
{
    [SerializeField]
    GameItemFactory factory;

    public Action onItemAddedToCatalog;
    public Action onItemRemovedFromCatalog;
    public void AddItemToCatalog(Item item)
    {
        InventoryObject iObj = Catalog.Instance.GetInventoryObject(item.PrefabId);

        if (iObj == null)
        {
            iObj = factory.CreateInventoryObject(item.PrefabId, item.Categories);
        }
        else
        {
            iObj.Amount++;
        }
        Catalog.Instance.Add(iObj);
        onItemAddedToCatalog?.Invoke();
    }

    public Item PullItemFromCatalog(InventoryObject iObj)
    {
        InventoryObject obj = Catalog.Instance.GetInventoryObject(iObj.PrefabId);

        if (obj == null)
        {
            Debug.LogError("trying to pull item that does not exist from catalog!");
            return null;
        }

        if (iObj.Amount == 1)
        {
            Catalog.Instance.Remove(iObj);
        }

        if (iObj.Amount > 1)
        {
            obj.Amount--;
        }
        onItemRemovedFromCatalog?.Invoke();
        return factory.CreateGridItem(iObj);

    }


}