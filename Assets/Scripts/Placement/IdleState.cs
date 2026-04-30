

public class IdleState : IPlacementState
{
    public void OnClick(PlacementManager manager)
    {
        manager.PickUpItemUnderMouse();
    }

    public void OnUpdate(PlacementManager manager)
    {
        // nothing happens while idle
    }
}


