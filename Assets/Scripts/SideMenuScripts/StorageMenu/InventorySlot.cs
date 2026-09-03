

using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Transform itemImage;

    Image itemBackgroundImage;

    [SerializeField]
    Transform itemHoverFrame;

    [SerializeField]
    Transform amountText;

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

        if (amountText == null)
        {
            Debug.Log("InventroySlot does not have an amount text!");
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
            }
            RectTransform rtAmount = amountText.GetComponent<RectTransform>();
            if (rtAmount != null)
            {
                rtAmount.sizeDelta = slotSize;
            }
            TextMeshProUGUI text = amountText.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = item.Amount.ToString();
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