using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CategoryInventory : MonoBehaviour
{

    [SerializeField]
    GameObject InventorySlotPrefab;

    List<GameObject> inventorySlots;
    GridLayoutGroup gridLayoutGroup;
    RectTransform rt;
    public void Awake()
    {
        if (InventorySlotPrefab == null)
        {
            Debug.Log("InventorySlot not assigned. Did you forget to reference it in storage menu?");
        }
        inventorySlots = new List<GameObject>();
        gridLayoutGroup = transform.GetComponent<GridLayoutGroup>();
        rt = transform.GetComponent<RectTransform>();

        float sizeOfSlot = (rt.rect.width - gridLayoutGroup.padding.left - gridLayoutGroup.padding.right - gridLayoutGroup.spacing.x * gridLayoutGroup.constraintCount)
            / (float)gridLayoutGroup.constraintCount;

        Debug.Log("constraint is " + (float)gridLayoutGroup.constraintCount + " and width is " + rt.rect.width);
        gridLayoutGroup.cellSize = new Vector2(sizeOfSlot, sizeOfSlot);
    }
    public void Add(GameObject item)
    {
        GameObject slot = Instantiate(InventorySlotPrefab, transform);
        Transform objectForItemImage = slot.transform.Find("ItemImage");
        Image img = objectForItemImage.GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("no image component on inventory slot prefab");
            return;
        }

        img.sprite = item.GetComponent<SpriteRenderer>().sprite;

        Vector2 slotSize = InventorySlotPrefab.GetComponent<RectTransform>().sizeDelta;
        RectTransform imgSize = img.GetComponent<RectTransform>();
        imgSize.sizeDelta = slotSize;

        inventorySlots.Add(slot);
    }
}