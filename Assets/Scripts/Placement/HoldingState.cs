using UnityEngine;

public class HoldingState : IPlacementState
{
    public void OnClick(PlacementManager manager)
    {
        manager.PlacePickedUpItem();
    }

    public void OnUpdate(PlacementManager manager)
    {
        manager.UpdatePositionOfPickedUpObject();
    }
}