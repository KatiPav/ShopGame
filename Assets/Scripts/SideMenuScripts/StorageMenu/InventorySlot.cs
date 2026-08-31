

using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Transform itemImage;

    Image itemBackgroundImage;

    [SerializeField]
    Transform itemHoverFrame;

    InventoryObject obj;
    public Action<InventoryObject> onSlotClicked;

    public void Awake()
    {
        itemBackgroundImage = GetComponent<Image>();
        if (itemBackgroundImage == null)
        {
            Debug.Log("InventroySlot does not have a background image!");
        }

        if (itemHoverFrame == null)
        {
            Debug.Log("InventroySlot does not have a on hover image!");
        }

        if (itemImage == null)
        {
            Debug.Log("InventroySlot does not have an item image!");
        }
    }

    public void Initialize(InventoryObject item, Vector2 slotSize = default(Vector2))
    {
        obj = item;
        gameObject.SetActive(false);

        Image img = itemImage.GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("no image component on the referenced Item image");
            return;
        }
        img.sprite = item.ObjectSprite;

        if (slotSize != default)
        {
            RectTransform rt = itemImage.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = slotSize * 0.70f;
            }

            RectTransform rtFrame = itemHoverFrame.GetComponent<RectTransform>();
            if (rtFrame != null)
            {
                rtFrame.sizeDelta = slotSize;
                Debug.Log("the frame size delta is" + rtFrame.sizeDelta);
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        onSlotClicked.Invoke(obj);
        Debug.Log("slot ckicked");
    }


    public void Enable()
    {
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(true);
    }

    public void Outline()
    {
        itemHoverFrame.gameObject.SetActive(true);
    }

    public void RemoveOutline()
    {
        itemHoverFrame.gameObject.SetActive(false);

    }
}