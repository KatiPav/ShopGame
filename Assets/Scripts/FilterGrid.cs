using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class FilterGrid : MonoBehaviour
{
    //[SerializeField]
    bool isIsometric = true;
    SpriteRenderer sr;
    float gridXsize = 1;
    float gridYsize = 1;
    int xCount = 0;
    int yCount = 0;

    [SerializeField]
    List<bool> grid = null;

    //monoBehaviour should not have init functions, so i have no idea
    // how the initialization should happen here. this is my workaround
    [SerializeField]
    private bool initialized = false;

    void OnValidate()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

        if (isIsometric)
        {
            gridXsize = 1;
            gridYsize = 0.5f;
            yCount = Mathf.CeilToInt((sr.sprite.rect.height / sr.sprite.pixelsPerUnit) * 2);
            xCount = Mathf.CeilToInt(sr.sprite.rect.width / sr.sprite.pixelsPerUnit) + 1;
        }
        else
        {
            gridXsize = 1;
            gridYsize = 1;
            yCount = Mathf.CeilToInt((sr.sprite.rect.height / sr.sprite.pixelsPerUnit));
            xCount = Mathf.CeilToInt(sr.sprite.rect.width / sr.sprite.pixelsPerUnit);
        }
        if (!initialized)
        {
            grid = Enumerable.Repeat(false, xCount * yCount).ToList();
            initialized = true;
        }
    }

    public void Click(int x, int y)
    {
        if (x < xCount && y < yCount && x >= 0 && y >= 0)
        {
            grid[xCount * y + x] = !grid[xCount * y + x];
        }
    }

    public void Select(int x, int y)
    {
        if (x < xCount && y < yCount && x >= 0 && y >= 0)
        {
            grid[xCount * y + x] = true;
        }
    }

    public void Deselect(int x, int y)
    {
        if (x < xCount && y < yCount && x >= 0 && y >= 0)
        {
            grid[xCount * y + x] = false;
        }
    }

    public bool isSelected(int x, int y)
    {
        return grid[xCount * y + x];
    }

    public int getWidth() { return xCount; }
    public int getHeight() { return yCount; }

    public bool IsIsometric() { return isIsometric; }

    public float getGridXSize() { return gridXsize; }
    public float getGridYSize() { return gridYsize; }

}