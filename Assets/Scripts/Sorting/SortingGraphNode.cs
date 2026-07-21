using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SortingGraphNode
{
    SpriteRenderer renderer;
    Vector2 floorPrintCenterPoint;
    public List<SortingGraphNode> edges;

    public SortingGraphNode(Item item)
    {
        renderer = item.gameObject.GetComponent<SpriteRenderer>();

        if (renderer == null)
        {
            Debug.Log("Error!!!! Cant sort object, it doesnt have a renderer!");
        }

        //TODO: calculate edges here based on overlap


    }

    public void UpdateEdges()
    {
        List<GameObject> overlaps = Physics2D.OverlapAreaAll(renderer.bounds.min, renderer.bounds.max)
        .Select(collider => collider.gameObject)
        .ToList();

        foreach (var obj in overlaps)
        {
            //if(/*compare them here*/){}
        };
    }


    bool ShouldBeOnTopOf()
    {
        return true;
    }
    public void SetSortOrderOfNode(int sortOrder)
    {
        renderer.sortingOrder = sortOrder;
    }
}