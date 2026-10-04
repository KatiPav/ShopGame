using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{

    [SerializeField]
    Tilemap wallTilemap;

    [SerializeField]
    Tilemap floorTilemap;


    public bool CanPlaceItemOnTilemap(Item item, Vector2Int coords)
    {
        foreach (Vector2Int cell in item.shape.GetCellsWithOrigin(coords))
        {
            Vector3Int pos = new Vector3Int(cell.x, cell.y, 0);

            switch (item.ItemType)
            {
                case Category.Furniture:
                    if (!floorTilemap.HasTile(pos)) return false;
                    break;
                case Category.Decoration:
                    if (!floorTilemap.HasTile(pos)) return false;
                    break;
            }
        }

        return true;
    }
}