using UnityEngine;



public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    Grid isometricGrid;

    GameObject pickedUpItem;

    public GridRegistry gridRegistry { get; private set; }

    IPlacementState currentState;
    Vector3Int lastPosition = new Vector3Int(0, 0, 0);

    void Awake()
    {
        gridRegistry = new GridRegistry();
        currentState = new IdleState();
    }

    void Update()
    {
        currentState.OnUpdate(this);
    }

    public void UpdatePositionOfPickedUpObject()
    {
        Vector2Int coords = GetCellCoordinatesOfMousePosition();
        Vector3 objPosition = isometricGrid.GetCellCenterWorld(new Vector3Int(coords.x, coords.y, 0));
        if (objPosition == lastPosition)
        {
            return;
        }

        if (gridRegistry.HasPlacedItem(coords))
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
        Vector3Int coordinates = isometricGrid.WorldToCell(new Vector3(worldPos.x, worldPos.y, 0));
        return new Vector2Int(coordinates.x, coordinates.y);
    }

    public Vector2Int WorldToCell(Vector3 worldpos)
    {
        Vector3Int coordinates = isometricGrid.WorldToCell(new Vector3(worldpos.x, worldpos.y, 0));
        return new Vector2Int(coordinates.x, coordinates.y);
    }

    public void PlaceItemObjectAt(GameObject item, Vector2Int position)
    {
        item.GetComponent<Item>().GridCoordinates = new Vector2Int(position.x, position.y);
        Vector3 worldPosition = isometricGrid.GetCellCenterWorld(new Vector3Int(position.x, position.y, 0));
        item.transform.position = worldPosition;

        gridRegistry.AddItem(item);
    }


}

