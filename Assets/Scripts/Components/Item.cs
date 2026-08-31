using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Item : MonoBehaviour
{
    private ItemRuntimeSet itemRuntimeSet;

    [SerializeField]
    public ItemType ItemType;

    public Guid Id { get; set; }
    public Vector2Int GridCoordinates { get; set; }
    public int PrefabId { get; set; }

    public FloorShape FloorShape { get; set; }
    private Grid GameGrid { get; set; }

    Collider2D Collider2D { get; set; }
    private SpriteRenderer sr;

    public bool ItemIsInPreview = false;


    void Awake()
    {
        Id = Guid.NewGuid();
        FloorShape = GetComponent<FloorShape>();
        if (FloorShape == null)
        {
            Debug.Log("Item does not have a floor shape!");
        }

        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.Log("Item does not have a Sprite Renderer!");
        }

        GameGrid = FindAnyObjectByType<Grid>();
        if (GameGrid == null)
        {
            Debug.Log("Item could not find the grid.");
        }

    }

    public void Initialize(int prefabId, Vector2Int coordinates, ItemRuntimeSet itemRuntimeSet)
    {
        this.itemRuntimeSet = itemRuntimeSet;
        PrefabId = prefabId;
        GridCoordinates = coordinates;

        itemRuntimeSet.Add(this);

    }

    private void OnDisable() => itemRuntimeSet?.Remove(this);

    public void SetPreview(bool state)
    {
        ItemIsInPreview = state;
        if (state)
        {
            sr.sortingOrder = 1000;
        }
    }

    public void MoveTo(Vector2Int coords, GridConverter gridConverter)
    {
        Vector3 worldPos = gridConverter.GridCoordsToWorldCoords(coords);

        gameObject.transform.position = gridConverter.GridCoordsToWorldCoords(coords);
        GridCoordinates = coords;
    }



    public Vector2Int GetMinXSquare()
    {
        return FloorShape.GetMinXWithOrigin(GridCoordinates);
    }

    public Vector2Int GetMinYSquare()
    {
        return FloorShape.GetMinYWithOrigin(GridCoordinates);
    }
    public Vector2Int GetMaxXSquare()
    {
        return FloorShape.GetMaxXWithOrigin(GridCoordinates);
    }
    public Vector2Int GetMaxYSquare()
    {
        return FloorShape.GetMaxYWithOrigin(GridCoordinates);
    }

}