using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryObject
{
    public Guid Id { get; private set; }

    public int PrefabId { get; private set; }

    public Sprite ObjectSprite { get; private set; }

    public int Amount { get; set; }

    public CatalogCategory CatalogCat { get; private set; }

    public InventoryObject(Guid id, int prefabId, Sprite sprite, int amount, CatalogCategory category)
    {
        Id = id;
        PrefabId = prefabId;
        ObjectSprite = sprite;
        Amount = amount;
        CatalogCat = category;

    }
}