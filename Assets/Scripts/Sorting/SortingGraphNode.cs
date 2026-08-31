using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SortingGraphNode : MonoBehaviour
{
    Item item;
    SpriteRenderer spRenderer;

    public List<SortingGraphNode> edges;

    void Awake()
    {
        item = gameObject.GetComponent<Item>();
        spRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (item == null)
        {
            Debug.Log("Item component not found!");
        }
        if (spRenderer == null)
        {
            Debug.Log("Error!!!! Cant sort object, it doesnt have a renderer!");
        }

        edges = new List<SortingGraphNode>();
    }

    public Bounds GetSpRendererBounds()
    {
        return spRenderer.bounds;
    }


    public SortingGraphNode RemoveFromEdges(SortingGraphNode nodeToRemove)
    {
        edges.Remove(nodeToRemove);
        return nodeToRemove;
    }

    public void UpdateEdgesWithOverlaps(List<SortingGraphNode> overlaps)
    {
        edges.Clear();
        foreach (var node in overlaps)
        {
            Item otherItem = node.gameObject.GetComponent<Item>();
            if (gameObject == otherItem.gameObject)
            {
                continue;
            }

            //if the object should be on top of the other then an edge should exist
            if (ShouldBeOnTop(item, otherItem))
            {

                edges.Add(node);
            }
        }
    }


    bool ShouldBeOnTop(Item item, Item otherItem)
    {
        float minX = item.GetMinXSquare().x - 0.5f;
        float minY = item.GetMinYSquare().y - 0.5f;
        float maxX = item.GetMaxXSquare().x + 0.5f;
        float maxY = item.GetMaxYSquare().y + 0.5f;

        float otherMinX = otherItem.GetMinXSquare().x - 0.5f; //plus half a square 
        float otherMinY = otherItem.GetMinYSquare().y - 0.5f;
        float otherMaxX = otherItem.GetMaxXSquare().x + 0.5f;
        float otherMaxY = otherItem.GetMaxYSquare().y + 0.5f;

        // Debug.Log("Comparing " + item.gameObject.name + " and  oth " + otherItem.gameObject.name);
        // Debug.Log(item.name + " has minx " + minX + "and miny " + minY);
        // Debug.Log(otherItem.name + " has othminx " + otherMinX + "and othminy " + otherMinY);

        // Debug.Log(item.name + " has maxx " + maxX + "and maxy " + maxY);
        // Debug.Log(otherItem.name + " has othmaxx " + otherMaxX + "and othmaxy " + otherMaxY);

        //we check on which axis the two objects do not intersect, 
        // if they do not then you can add a plane separating them

        // test for intersection x-axis
        // (lower x value is in front)
        if (minX >= otherMaxX)
        {
            return false;
        }
        else if (otherMinX >= maxX)
        {
            return true;
        }

        // test for intersection y-axis
        // (lower y value is in front)
        if (minY >= otherMaxY)
        {

            return false;
        }
        else if (otherMinY >= maxY)
        {
            return true;
        }

        Debug.Log("Sorting graph node has weird error. This should never happen. Object seems to have negative size??");

        return false;
    }

    public void SetSortOrderOfNode(int sortOrder)
    {
        spRenderer.sortingOrder = sortOrder;
    }
}