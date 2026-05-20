using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    Vector2Int lastCoordinates = new Vector2Int(0, 0);

    void Awake()
    {
        currentState = new IdleState();
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

        if (!gridRegistry.CanPlaceItemAt(coords)) //do we want to move it on the mouse even if not possible to place there?
        {
            return;
        }

        if (!tilemapManager.CanPlaceItemOnTilemap(pickedUpItem, coords))
        {
            return;
        }

        pickedUpItem.MoveTo(coords, gridConverter);
        lastCoordinates = coords;

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

