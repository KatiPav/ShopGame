using UnityEngine;

public class PlacementManager : MonoBehaviour
{

    [SerializeField]
    GridRegistry gridRegistry;

    [SerializeField]
    GridCoordinates gridCoordinates;

    GameObject pickedUpItem = null;
    IPlacementState currentState;

    Vector3Int lastPosition = new Vector3Int(0, 0, 0);

    void Awake()
    {
        currentState = new IdleState();
    }

    void Update()
    {
        currentState.OnUpdate(this);
    }

    public void UpdatePositionOfPickedUpObject()
    {
        Vector2Int coords = GetCellCoordinatesOfMousePosition();
        Vector3 objPosition = gridCoordinates.GridCoordsToWorldCoords(coords);

        if (objPosition == lastPosition)
        {
            return;
        }

        if (gridRegistry.HasPlacedItem(coords))
        {
            return;
        }

        if (pickedUpItem == null)
        {
            return;
        }

        pickedUpItem.transform.position = objPosition;
        lastPosition = new Vector3Int(coords.x, coords.y, 0);

    }

    public void Click()
    {
        currentState.OnClick(this);
    }

    public void PlacePickedUpItemAtMousePosition()
    {
        Vector2Int coords = GetCellCoordinatesOfMousePosition();
        if (gridRegistry.HasPlacedItem(coords))
        {
            Debug.Log("Cant place here!");
            return;
        }

        PlaceItemObjectAt(pickedUpItem, coords);

        currentState = new IdleState();

    }

    private bool TryGetItemUnderMouse(out GameObject item)
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
            Debug.Log("Item under mouse does not have Item component.");
            item = null;
            return false;
        }
        item = hit.transform.gameObject;
        return true;
    }

    public void PickUpItemUnderMouse()
    {
        GameObject gObj;

        if (!TryGetItemUnderMouse(out gObj))
        {
            return;
        }

        pickedUpItem = gridRegistry.PullItem(gObj);
        currentState = new HoldingState();
    }


    private Vector2Int GetCellCoordinatesOfMousePosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return gridCoordinates.WorldCoordsToGridCoords(worldPos);
    }

    public void PlaceItemObjectAt(GameObject item, Vector2Int position)
    {
        item.SetActive(true);
        item.GetComponent<Item>().GridCoordinates = new Vector2Int(position.x, position.y);
        item.transform.position = gridCoordinates.GridCoordsToWorldCoords(position);

        gridRegistry.AddItem(item);
    }

}

