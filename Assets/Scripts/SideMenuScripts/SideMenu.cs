using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    [SerializeField]
    InventorySlot InventorySlotPrefab;

    [SerializeField]
    PlacementManager PlacementManager;

    [SerializeField]
    CatalogController catalogController;

    [SerializeField]
    Category defaultCategory = Category.Furniture;

    GridLayoutGroup layout;

    RectTransform rt;

    List<InventorySlot> slots = new List<InventorySlot>();

    Vector2 slotSize;
    public void Awake()
    {
        layout = transform.GetComponent<GridLayoutGroup>();
        if (layout == null)
        {
            Debug.Log("Side menu does not have a Layout. Objects may be positioned wrong. Expecting a GridLayoutGroup component.");
        }

        rt = GetComponent<RectTransform>();
        if (rt == null)
        {
            Debug.Log("Side menu does not have a Rect Transform. Objects may be positioned wrong.");
        }

        catalogController.onItemAddedToCatalog += HandleItemAdded;
        catalogController.onItemRemovedFromCatalog += HandleItemRemoved;

        ResizeLayout();
    }

    public void OnDisable()
    {
        //catalogController.onItemAddedToCatalog -= HandleItemAdded;
    }

    public void Start()
    {
        CreateInventorySlots();

    }

    private void CreateInventorySlots()
    {
        DestroySlots();

        foreach (var item in Catalog.Instance.GetObjectsOfCategory(defaultCategory))
        {
            CreateInventorySlot(item);
        }
    }

    private void HandleItemAdded()
    {
        //TODO: fix this so no need to refresh the whole menu
        DestroySlots();
        CreateInventorySlots();

    }
    private void HandleItemRemoved()
    {
        //TODO: fix this so no need to refresh the whole menu
        CreateInventorySlots();

    }

    private void DestroySlots()
    {
        foreach (var s in slots)
        {
            s.gameObject.SetActive(false);
            Destroy(s.gameObject);
        }

        slots.Clear();
    }

    private void CreateInventorySlot(InventoryObject obj)
    {
        if (obj.Categories.Contains(defaultCategory))
        {
            InventorySlot slot = Instantiate(InventorySlotPrefab);
            slot.transform.SetParent(gameObject.transform, false);
            slot.Initialize(obj, slotSize);
            slot.onSlotClicked += HandleSlotClicked;
            slots.Add(slot);
            slot.Enable();
        }
    }

    private void HandleSlotClicked(InventoryObject obj)
    {
        Item spawned = catalogController.PullItemFromCatalog(obj);

        PlacementManager.SetPickedItem(spawned);
    }

    private void ResizeLayout()
    {
        float sizeOfSlot = (rt.rect.width - layout.padding.left - layout.padding.right - layout.spacing.x * (layout.constraintCount - 1))
    / (float)layout.constraintCount;

        sizeOfSlot = HalfRound(sizeOfSlot);

        layout.cellSize = new Vector2(sizeOfSlot, sizeOfSlot);
        slotSize = layout.cellSize;
    }

    public void DisplayObjects()
    {
        foreach (var s in slots)
        {
            s.Enable();
        }
    }

    static float HalfRound(float num) => System.MathF.Round(num * 2f) / 2f;
}