using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Item : MonoBehaviour
{
    [SerializeField]
    public ItemType ItemType;

    public Guid Id { get; set; }
    public Vector2Int GridCoordinates { get; set; }
    public int PrefabId { get; set; }

    public FloorShape FloorShape { get; set; }
    private Grid GameGrid { get; set; }

    Collider2D Collider2D { get; set; }

    Rigidbody2D RB { get; set; }


    void Awake()
    {
        Id = Guid.NewGuid();
        FloorShape = GetComponent<FloorShape>();
        RB = GetComponent<Rigidbody2D>();
        GameGrid = FindAnyObjectByType<Grid>();
        if (GameGrid == null)
        {
            Debug.Log("Item could not find the grid.");
        }

        if (RB == null)
        {
            Debug.Log("Item could not find its RigidBody2D.");
        }
    }

    public void Initialize(int prefabId, Vector2Int coordinates)
    {
        PrefabId = prefabId;
        GridCoordinates = coordinates;

        //CalculateMeasurments();
    }

    public void MoveTo(Vector2Int coords, GridConverter gridConverter)
    {
        Vector3 worldPos = gridConverter.GridCoordsToWorldCoords(coords);
        //RB.MovePosition(new Vector2(worldPos.x, worldPos.y)); 
        // TODO: fix this as soon as possible, 
        // treat it as a physics object and remove the SyncTransforms in Sorting!!!


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
    // private void CalculateMeasurments(){
    //     Vector2Int back;
    //     Vector2Int front;
    //     Vector2Int left;
    //     Vector2Int right;

    //     Collider2D.max.y;
    //     //take highest point 
    //     //check in which grid sqr it is 
    //     //check difference betwee n floor back and it

    //     foreach (Vector2Int v in FloorShape.GetFloorCells())
    //     {
    //         Debug.Log("Floor cell is " + v);

    //     }


    // }
}