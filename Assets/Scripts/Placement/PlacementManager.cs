
using UnityEngine;
using System;
using System.Collections.Generic;


public class PlacementManager : MonoBehaviour
{

    [SerializeField]
    GridRegistry gridRegistry;

    [SerializeField]
    TilemapManager tilemapManager;

    [SerializeField]
    MovementManager movementManager;

    [SerializeField]
    GridConverter gridConverter;

    Item pickedUpItem = null;
    bool IsHolding => pickedUpItem != null;

    Vector2Int originalCoordintes = default;
    Vector2Int lastCoordinates = new Vector2Int(0, 0);
    bool hasLastCoordinates;

    void Awake()
    {

        if (gridRegistry == null)
        {
            Debug.Log("Grid registry is not set!");
            return;
        }
        if (tilemapManager == null)
        {
            Debug.Log("Tilemap manager is not set!");
            return;
        }
        if (movementManager == null)
        {
            Debug.Log("Movement manager is not set!");
            return;
        }
        if (gridConverter == null)
        {
            Debug.Log("Grid Converter is not set!");
            return;
        }
    }

    void Update()
    {
        if (IsHolding)
            UpdatePositionOfPickedUpObject();
    }
    public void Click()
    {
        if (IsHolding)
            PlaceHeldItem();
        else
            PickUpItem();
    }

    public void UpdatePositionOfPickedUpObject()
    {
        Vector2Int coords = gridConverter.GetGridCoordinatesOfMousePosition();

        if (pickedUpItem == null)
        {
            return;
        }
        if (coords == lastCoordinates)
        {
            return;
        }
        if (!gridRegistry.CanPlaceItemInOrAraoundCoords(pickedUpItem, coords, out coords)
        || !tilemapManager.CanPlaceItemOnTilemap(pickedUpItem, coords))
        {
            MakeItemPreview();
            return;
        }

        pickedUpItem.SetPreview(false);
        Vector2Int oldCoords = pickedUpItem.GridCoordinates;
        movementManager.RequestMove(pickedUpItem, oldCoords, coords);
        lastCoordinates = coords;
    }

    private void MakeItemPreview() //this whole flow is mazalo needs to be fixed and cleaned
    {
        Vector3 mouseScreenPos = Input.mousePosition;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 10;
        pickedUpItem.gameObject.transform.position = worldPos;
        pickedUpItem.SetPreview(true);
    }



    public void PlaceHeldItem()
    {
        if (pickedUpItem == null)
        {
            return;
        }

        pickedUpItem.gameObject.SetActive(true);
        movementManager.RequestMove(pickedUpItem, pickedUpItem.GridCoordinates, lastCoordinates);
        gridRegistry.AddItem(pickedUpItem);

        pickedUpItem = null;
        hasLastCoordinates = false;
        originalCoordintes = default;

    }

    private bool TryPickUpItemUnderMouse(out Item item)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y), Vector2.zero);//add layer mask later

        if (!hit)
        {
            item = null;
            return false;
        }

        if (hit.transform.gameObject.GetComponent<Item>() == null)
        {
            Debug.Log("Object under mouse does not have Item component.");
            item = null;
            return false;
        }

        item = hit.transform.gameObject.GetComponent<Item>();
        return true;
    }

    public void PickUpItem()
    {
        Item item;

        if (!TryPickUpItemUnderMouse(out item))
        {
            return;
        }
        originalCoordintes = item.GridCoordinates;
        pickedUpItem = gridRegistry.PullItem(item);
    }

    public void SetPickedItem(Item item)
    {
        if (IsHolding && originalCoordintes != default)
        {
            ReturnToLastCoordinates(pickedUpItem);
        }
        else if (IsHolding && originalCoordintes == default)
        {
            ReturnToInventory(pickedUpItem);
        }

        pickedUpItem = item;
        Debug.Log("item was created and is currently held?");
    }

    private void ReturnToInventory(Item item)
    {
        //Catalog.Instance.Add(item);
    }

    private void ReturnToLastCoordinates(Item item)
    {
        movementManager.RequestMove(item, item.GridCoordinates, originalCoordintes);
        gridRegistry.AddItem(item);
    }



}

