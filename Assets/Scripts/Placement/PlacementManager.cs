using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MoveRequest
{
    public Item item;
    public Vector2Int olgGridCoordinates;
    public Vector2Int newGridCoordinates;

    public MoveRequest(Item item, Vector2Int oldCoords, Vector2Int newCoords)
    {
        this.item = item;
        olgGridCoordinates = oldCoords;
        newGridCoordinates = newCoords;
    }
}


public class PlacementManager : MonoBehaviour
{

    [SerializeField]
    GridRegistry gridRegistry;

    [SerializeField]
    GridConverter gridConverter;

    [SerializeField]
    TilemapManager tilemapManager;

    private List<MoveRequest> moveRequests = new List<MoveRequest>();
    private List<MoveRequest> appliedThisStep = new List<MoveRequest>();
    Item pickedUpItem = null;
    IPlacementState currentState;

    public Action<Item, Vector2Int, Vector2Int> OnItemMoved;

    Vector2Int lastCoordinates = new Vector2Int(0, 0);

    void Awake()
    {
        currentState = new IdleState();

        if (tilemapManager == null)
        {
            Debug.Log("Tilemap manager is not set!");
            return;
        }
    }

    void Update()
    {
        currentState.OnUpdate(this);
    }
    public void Click()
    {
        currentState.OnClick(this);
    }

    public void UpdatePositionOfPickedUpObject()
    {
        Vector2Int coords = GetCellCoordinatesOfMousePosition();

        if (pickedUpItem == null)
        {
            return;
        }

        if (coords == lastCoordinates)
        {
            return;
        }

        if (!CanPlaceInOrAraound(coords, out coords))
        {
            return;
        }

        if (!tilemapManager.CanPlaceItemOnTilemap(pickedUpItem, coords))
        {
            return;
        }

        Vector2Int oldCoords = pickedUpItem.GridCoordinates;
        moveRequests.Add(new MoveRequest(pickedUpItem, oldCoords, coords));
        lastCoordinates = coords;
    }

    void FixedUpdate()
    {
        foreach (var req in moveRequests)
        {
            req.item.MoveTo(req.newGridCoordinates, gridConverter);
        }
        appliedThisStep.AddRange(moveRequests);
        moveRequests.Clear();
    }

    void LateUpdate()
    {
        foreach (var req in appliedThisStep)
        {
            OnItemMoved?.Invoke(req.item, req.olgGridCoordinates, req.newGridCoordinates);
        }
        appliedThisStep.Clear();
    }

    private bool CanPlaceInOrAraound(Vector2Int coords, out Vector2Int outCoords)
    {
        Vector2Int[] offsets =
{
            new Vector2Int(0,0),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1)
        };

        bool canPlace = false;
        outCoords = coords;
        foreach (Vector2Int offset in offsets)
        {
            Vector2Int newCoords = coords + offset;

            if (gridRegistry.CanPlaceItemAt(newCoords, pickedUpItem))
            {
                canPlace = true;
                outCoords = newCoords;
                break;
            }
        }
        return canPlace;
    }
    public void PlacePickedUpItem()
    {
        pickedUpItem.gameObject.SetActive(true);
        pickedUpItem.MoveTo(lastCoordinates, gridConverter);

        gridRegistry.AddItem(pickedUpItem);
        currentState = new IdleState();
    }

    private bool TryGetItemUnderMouse(out Item item)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y), Vector2.zero);//add layer mask later

        if (!hit)
        {
            Debug.Log("No colliders under mouse");
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

        if (!TryGetItemUnderMouse(out item))
        {
            return;
        }

        pickedUpItem = gridRegistry.PullItem(item);
        currentState = new HoldingState();
    }


    private Vector2Int GetCellCoordinatesOfMousePosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return gridConverter.WorldCoordsToGridCoords(worldPos);
    }
}

