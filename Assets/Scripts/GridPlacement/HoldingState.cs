using UnityEngine;

public class HoldingState : IPlacementState
{
    public void OnClick(PlacementManager manager)
    {
        manager.PlacePickedUpItemAtMousePosition();
    }

    public void OnUpdate(PlacementManager manager)
    {
        manager.UpdatePositionOfPickedUpObject();
    }
}