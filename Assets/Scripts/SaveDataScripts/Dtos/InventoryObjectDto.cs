public class InventoryObjectDto : IObjectDto
{
    public int PrefabId { get; set; }

    public ItemType ItemType { get; set; }
    public int amount;
}