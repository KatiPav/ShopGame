using UnityEngine;
using System.Collections.Generic;


public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    Grid isometricGrid;

    GameObject selectedObject;

    FurnitureGridData furnitureGridData = new FurnitureGridData();
    DecorationsGridData decorationsGridData = new DecorationsGridData();

    bool objectSelected = false;
    Vector3Int lastPosition = new Vector3Int(0, 0, 0);

    void Update()
    {
        if (objectSelected)
        {
            Vector2Int coords = GetCellCoordinatesOfMousePosition();
            Vector3 objPosition = isometricGrid.CellToWorld(new Vector3Int(coords.x, coords.y, 0));
            if (objPosition != lastPosition)
            {
                selectedObject.transform.position = objPosition;
            }
        }
    }

    public void Click()
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
        Vector2Int coords = GetCellCoordinatesOfMousePosition();
        LoadObjectInGame(selectedObject, coords);

        objectSelected = false;
        Debug.Log("obj placed");
    }

    void PickUpObject()
    {
        Vector2Int coords = GetCellCoordinatesOfMousePosition();

        Debug.Log("trying to pick up obj at " + coords);

        if (!decorationsGridData.TryPickUpObject(coords, out selectedObject))
        {
            if (!furnitureGridData.TryPickUpObject(coords, out selectedObject))
            {
                Debug.Log("no object to select");
                return;
            }
        }

        objectSelected = true;
        Debug.Log("obj picked up");
    }

    private Vector2Int GetCellCoordinatesOfMousePosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinates = isometricGrid.WorldToCell(new Vector3(worldPos.x, worldPos.y, 0));
        return new Vector2Int(coordinates.x, coordinates.y);
    }

    public void LoadObjectInGame(GameObject obj, Vector2Int coordinates)
    {
        Vector3 worldPosition = isometricGrid.CellToWorld(new Vector3Int(coordinates.x, coordinates.y, 0));
        obj.transform.position = worldPosition;
        AddToAppropriateGrid(obj, coordinates);
        Debug.Log(obj + "obj should be added to " + coordinates + " at world pos " + worldPosition);
    }

    private void AddToAppropriateGrid(GameObject obj, Vector2Int coords)
    {
        switch (obj.layer)
        {
            case 6://rename these to understandable constants or enums
                furnitureGridData.TryPlaceObject(obj, new Vector2Int(coords.x, coords.y));
                break;
            case 7:
                decorationsGridData.TryPlaceObject(obj, new Vector2Int(coords.x, coords.y));
                break;
        }
    }

}

