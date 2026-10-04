using UnityEngine;

public class ReturnToInventoryHotspot : MonoBehaviour
{
    [SerializeField]
    PlacementManager placementManager;

    [SerializeField]
    CatalogController catalogController;


    public void ReturnHeldItemToInventory() //maybe needs differnt 
    {
        Item item = placementManager.GiveOwnershipOfHeldItem();
        if (item == null)
        {
            Debug.LogError("Item is null?");
        }

        catalogController.AddItemToCatalog(item);
        Destroy(item.gameObject);
    }

}