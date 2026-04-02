using UnityEngine;
using System.Collections.Generic;
using System;


public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    Grid isometricGrid;

    GameObject selectedItem;

    FurnitureGridData furnitureGridData = new FurnitureGridData();
    DecorationsGridData decorationsGridData = new DecorationsGridData();

    bool objectSelected = false;
    Vector3Int lastPosition = new Vector3Int(0, 0, 0);

    void Update()
    {
        if (objectSelected)
        {
            UpdatePositionOfPickedUpObject();
        }
    }

    private void UpdatePositionOfPickedUpObject()
    {
        Vector2Int coords = GetCellCoordinatesOfMousePosition();
        Vector3 objPosition = isometricGrid.GetCellCenterWorld(new Vector3Int(coords.x, coords.y, 0));
        if (objPosition != lastPosition)
        {
            selectedItem.transform.position = objPosition;

            UpdateSortOrder(selectedItem);
        }
    }

    public void Click()
    {
        if (objectSelected)
        {
            Vector2Int coords = GetCellCoordinatesOfMousePosition();
            PlaceItemObjectAt(selectedItem, coords);

            objectSelected = false;
        }
        else
        {
            PickUpOItemUnderMouse();
        }
    }

    void PickUpOItemUnderMouse()
    {
        Vector2Int coords = GetCellCoordinatesOfMousePosition();

        //check which object from the screen we got. get the floor shape and get the 
        Debug.Log("trying to pick up obj at " + coords);

        GameObject i = decorationsGridData.PullItem(coords);
        if (i != null)
        {
            selectedItem = i;
            objectSelected = true;
            return;
        }

        i = furnitureGridData.PullItem(coords);
        if (i != null)
        {
            selectedItem = i;
            objectSelected = true;
            return;
        }

        objectSelected = false;
        Debug.Log("no object to pick up");
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
        Vector3 worldPosition = isometricGrid.GetCellCenterWorld(new Vector3Int(position.x, position.y, 0));
        item.transform.position = worldPosition;
        UpdateSortOrder(item);
        item.GetComponent<Item>().GridCoordinates = new Vector2Int(position.x, position.y);
        AddToAppropriateGrid(item);
    }

    private void UpdateSortOrder(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().sortingOrder = -((int)obj.transform.position.y * 1000 + (int)obj.transform.position.x);
    }

    private void AddToAppropriateGrid(GameObject item)
    {
        switch (item.layer)
        {
            case 7://rename these to understandable constants or enums
                furnitureGridData.AddItem(item);
                break;
            case 6:
                decorationsGridData.AddItem(item);
                break;
        }
    }

    public List<GameObject> getFurniture()
    {
        return furnitureGridData.getItems();
    }

    public List<GameObject> getDecorations()
    {
        return decorationsGridData.getItems();
    }

}

