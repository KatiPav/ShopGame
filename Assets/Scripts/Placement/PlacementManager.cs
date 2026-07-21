using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PlacementManager : MonoBehaviour
{

    [SerializeField]
    GridRegistry gridRegistry;

    [SerializeField]
    GridConverter gridConverter;

    [SerializeField]
    TilemapManager tilemapManager;

    Item pickedUpItem = null; 
    IPlacementState currentState;

    public Action<Item> OnItemMoved;

    Vector2Int lastCoordinates = new Vector2Int(0, 0);

    void Awake()
    {
        currentState = new IdleState();

        if(tilemapManager == null)
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

        pickedUpItem.MoveTo(coords, gridConverter);
        if(OnItemMoved != null) {
           // OnItemMoved(pickedUpItem);
        }
        else
        {
            Debug.Log("why tf is it null?");

        }
        lastCoordinates = coords;

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

