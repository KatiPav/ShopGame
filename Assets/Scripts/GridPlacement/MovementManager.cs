using UnityEngine;
using System.Collections.Generic;


public class MovementManager : MonoBehaviour
{
    [SerializeField]
    Grid isometricGrid;

    [SerializeField]
    InputManager inputManager;
    GameObject selectedObject;

    FurnitureGridData furnitureGridData = new FurnitureGridData();
    DecorationsGridData decorationsGridData = new DecorationsGridData();

    bool objectSelected = false;
    Vector3Int lastPosition = new Vector3Int(0, 0, 0);

    [SerializeField]
    List<GameObject> initialObjects;

    void TemporaryTest()
    {
        GameObject gObj = Instantiate(initialObjects[0], new Vector3Int(3, -4, 0), Quaternion.identity);
        gObj.layer = 6;
        Vector3Int coords = isometricGrid.WorldToCell(gObj.transform.position);
        Vector3 objPosition = isometricGrid.CellToWorld(coords);
        gObj.transform.position = objPosition;
        furnitureGridData.TryPlaceObject(gObj, new Vector2Int(coords.x, coords.y));


        GameObject gObj2 = Instantiate(initialObjects[1], new Vector3Int(3, -4, 0), Quaternion.identity);
        gObj2.layer = 7;
        Vector3Int coords2 = isometricGrid.WorldToCell(gObj2.transform.position);
        Vector3 objPosition2 = isometricGrid.CellToWorld(coords2);
        gObj2.transform.position = objPosition2;
        decorationsGridData.TryPlaceObject(gObj2, new Vector2Int(coords.x, coords.y));

    }


    void Start()
    {
        TemporaryTest();

        //subscribe to onClick
        inputManager.OnClick += Click;
        //add esc option that returns the object to previous position
    }

    void Update()
    {
        if (objectSelected)
        {
            Vector3Int coords = GetCellCoordinatesOfMousePosition();
            Vector3 objPosition = isometricGrid.CellToWorld(coords);
            if (objPosition != lastPosition)
            {
                selectedObject.transform.position = new Vector3(objPosition.x, objPosition.y, 0);
            }
        }
    }

    void Click()
    {
        if (objectSelected)
        {
            PlaceObject();
        }
        else
        {
            PickUpObject();
        }
    }

    void PlaceObject()
    {

        Vector3Int coords = GetCellCoordinatesOfMousePosition();
        Vector3 objPosition = isometricGrid.CellToWorld(coords);

        selectedObject.transform.position = objPosition;

        switch (selectedObject.layer)
        {
            case 6://rename these to understandable constants or enums
                furnitureGridData.TryPlaceObject(selectedObject, new Vector2Int(coords.x, coords.y));
                break;
            case 7:
                decorationsGridData.TryPlaceObject(selectedObject, new Vector2Int(coords.x, coords.y));
                break;
        }

        objectSelected = false;
        Debug.Log("obj placed");
    }

    void PickUpObject()
    {
        Vector3Int coords = GetCellCoordinatesOfMousePosition();

        Debug.Log("trying to pick up obj at " + coords);

        if (!decorationsGridData.TryPickUpObject(new Vector2Int(coords.x, coords.y), out selectedObject))
        {
            if (!furnitureGridData.TryPickUpObject(new Vector2Int(coords.x, coords.y), out selectedObject))
            {
                Debug.Log("no object to select");
                return;
            }
        }

        objectSelected = true;
        Debug.Log("obj picked up");
    }

    private Vector3Int GetCellCoordinatesOfMousePosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return isometricGrid.WorldToCell(new Vector3(worldPos.x, worldPos.y, 0));
    }

}

