using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{

    [SerializeField]
    Tilemap leftWallTilemap;

    [SerializeField]
    Tilemap floorTilemap;


    [SerializeField]
    Tilemap rightWallTilemap;


    public bool CanPlaceItemOnTilemap(Item item, Vector2Int coords)
    {
        Tilemap tm;
        if (item.planeCategory == PlaneCategory.Floor)
        {
            tm = floorTilemap;
        }
        else if (item.planeCategory == PlaneCategory.LeftWall)
        {
            tm = leftWallTilemap;
        }
        else
        {
            tm = rightWallTilemap;
        }

        foreach (Vector2Int cell in item.shape.GetCellsWithOrigin(coords))
        {
            Vector3Int pos = new Vector3Int(cell.x, cell.y, 0);

            if (!tm.HasTile(pos))
            {
                return false;
            }
        }

        return true;
    }
}