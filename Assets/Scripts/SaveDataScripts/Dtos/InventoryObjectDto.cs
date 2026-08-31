using System;
using System.Collections.Generic;

public class InventoryObjectDto : IObjectDto
{
    public Guid Id { get; set; }
    public int PrefabId { get; set; }

    public List<Category> Categories { get; set; }
    public int amount;
}