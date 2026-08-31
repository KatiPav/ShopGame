using UnityEngine;
using System.Collections.Generic;
using System;


public class MovementManager : MonoBehaviour
{
    [SerializeField]
    GridConverter gridConverter;


    public Action<Item, Vector2Int, Vector2Int> OnItemMoved;

    private List<MoveRequest> moveRequests = new List<MoveRequest>();
    private List<MoveRequest> appliedThisStep = new List<MoveRequest>();

    public void RequestMove(Item item, Vector2Int oldCoords, Vector2Int newCoords)
    {
        moveRequests.Add(new MoveRequest(item, oldCoords, newCoords));
    }

    void FixedUpdate()
    {
        foreach (var req in moveRequests)
        {
            req.item.MoveTo(req.newGridCoordinates, gridConverter);
        }
        appliedThisStep.AddRange(moveRequests);
        moveRequests.Clear();
    }

    void LateUpdate()
    {
        foreach (var req in appliedThisStep)
        {
            OnItemMoved?.Invoke(req.item, req.olgGridCoordinates, req.newGridCoordinates);
        }
        appliedThisStep.Clear();
    }
}