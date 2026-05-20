

public class IdleState : IPlacementState
{
    public void OnClick(PlacementManager manager)
    {
        manager.PickUpItem();
    }

    public void OnUpdate(PlacementManager manager)
    {
        // nothing happens while idle
    }
}


