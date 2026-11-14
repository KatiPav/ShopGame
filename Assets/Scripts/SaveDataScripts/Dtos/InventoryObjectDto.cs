using System;
using System.Collections.Generic;

public class InventoryObjectDto : IObjectDto
{
    public Guid Id { get; set; }
    public int PrefabId { get; set; }

    public int Amount { get; set; }

    public InventoryObjectDto(Guid id, int prefabId, int amount)
    {
        Id = id;
        PrefabId = prefabId;
        Amount = amount;
    }
}