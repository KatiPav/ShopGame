using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Item : MonoBehaviour
{
    private ItemRuntimeSet itemRuntimeSet;

    [SerializeField]
    public PlaneCategory planeCategory;

    [SerializeField]
    public CatalogCategory catalogCategory;

    public Guid Id { get; set; }
    public Vector2Int GridCoordinates { get; set; }
    public int PrefabId { get; set; }

    public ShapePrint shape { get; set; }
    private Grid GameGrid { get; set; }

    Collider2D Collider2D { get; set; }
    private SpriteRenderer sr;

    public bool ItemIsInPreview = false;


    void Awake()
    {
        Id = Guid.NewGuid();
        shape = GetComponent<ShapePrint>();
        if (shape == null)
        {
            Debug.Log("Item does not have a shape!");
        }

        //todo: add check if shape matches category

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

    public void Initialize(int prefabId, Vector2Int coordinates, ItemRuntimeSet itemRuntimeSet, CatalogCategory catalogCategory, PlaneCategory planeCategory)
    {
        this.itemRuntimeSet = itemRuntimeSet;
        PrefabId = prefabId;
        GridCoordinates = coordinates;
        this.catalogCategory = catalogCategory;
        this.planeCategory = planeCategory;

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

        gameObject.transform.position = gridConverter.GridCoordsToWorldCoords(coords, planeCategory);
        GridCoordinates = coords;
    }



    public Vector2Int GetMinXSquare()
    {
        return shape.GetMinXWithOrigin(GridCoordinates);
    }

    public Vector2Int GetMinYSquare()
    {
        return shape.GetMinYWithOrigin(GridCoordinates);
    }
    public Vector2Int GetMaxXSquare()
    {
        return shape.GetMaxXWithOrigin(GridCoordinates);
    }
    public Vector2Int GetMaxYSquare()
    {
        return shape.GetMaxYWithOrigin(GridCoordinates);
    }

}