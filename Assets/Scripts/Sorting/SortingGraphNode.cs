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

    public void Start()
    {
        UpdateEdges();
    }


    public SortingGraphNode RemoveFromEdges(SortingGraphNode nodeToRemove)
    {
        edges.Remove(nodeToRemove);
        return nodeToRemove;
    }

    public void SeparateNodeFromGraph()
    {
        List<Item> overlaps = Physics2D.OverlapAreaAll(spRenderer.bounds.min, spRenderer.bounds.max)
        .Select(collider => collider.gameObject.GetComponent<Item>())
        .ToList();

        foreach (var otherItem in overlaps)
        {
            if (otherItem == item)
            { //dont compare with itself
                continue;
            }
        }


    }

    public List<SortingGraphNode> GetOverlappingNodes()
    {
        List<SortingGraphNode> overlaps = Physics2D.OverlapAreaAll(spRenderer.bounds.min, spRenderer.bounds.max)
            .Select(collider => collider.gameObject.GetComponent<SortingGraphNode>())
            .Where(collider => collider.gameObject != gameObject)
            .Where(item => item != null)
            .ToList();

        return overlaps;
    }

    public void UpdateEdges()
    {
        List<SortingGraphNode> overlaps = GetOverlappingNodes();

        Debug.Log("Trying to update edges. Found overlaps: " + (overlaps.Count - 1));


        edges.Clear();
        foreach (var node in overlaps)
        {
            Item otherItem = node.gameObject.GetComponent<Item>();

            //if the object should be on top of the other then an edge should exist
            if (!ShouldBeBelow(item, otherItem))
            {
                if (node == null)
                {
                    Debug.Log("Comparison item does not have Sorting graph node component!");
                    return;
                }
                edges.Add(node);
                Debug.Log("edges has num of elements:" + edges.Count);
            }
        }

    }


    bool ShouldBeBelow(Item item, Item otherItem)
    {
        float minX = item.GetMinXSquare().x - 0.5f;
        float minY = item.GetMinYSquare().y - 0.5f;
        float maxX = item.GetMaxXSquare().x + 0.5f;
        float maxY = item.GetMaxYSquare().y + 0.5f;

        float otherMinX = otherItem.GetMinXSquare().x - 0.5f; //plus half a square 
        float otherMinY = otherItem.GetMinYSquare().y - 0.5f;
        float otherMaxX = otherItem.GetMaxXSquare().x + 0.5f;
        float otherMaxY = otherItem.GetMaxYSquare().y + 0.5f;

        Debug.Log("Comparing " + item.gameObject.name + " and  oth " + otherItem.gameObject.name);
        Debug.Log(item.name + " has minx " + minX + "and miny " + minY);
        Debug.Log(otherItem.name + " has othminx " + otherMinX + "and othminy " + otherMinY);

        Debug.Log(item.name + " has maxx " + maxX + "and maxy " + maxY);
        Debug.Log(otherItem.name + " has othmaxx " + otherMaxX + "and othmaxy " + otherMaxY);

        //we check on which axis the two objects do not intersect, 
        // if they do not then you can add a plane separating them

        // test for intersection x-axis
        // (lower x value is in front)
        if (minX >= otherMaxX)
        {
            Debug.Log("1");
            return false;
        }
        else if (otherMinX >= maxX)
        {
            Debug.Log("2");

            return true;
        }

        // test for intersection y-axis
        // (lower y value is in front)
        if (minY >= otherMaxY)
        {
            Debug.Log("3");

            return false;
        }
        else if (otherMinY >= maxY)
        {
            Debug.Log("4");

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