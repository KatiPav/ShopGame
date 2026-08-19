using UnityEngine;
using System.Collections.Generic;
using System.Linq;


//This graph is used to determine the order in which the items need to be drawn on the screen.
//The nodes are the game objects and there is a directed edge between 2 nodes if one needs to be on top of the other.
//It keeps track of any object movement and updates dynamically. 
//At the end it creates a topological ordering that is used to set the sortOrder in Unity.
public class SortingGraph : MonoBehaviour
{
    [SerializeField]
    PlacementManager placementManager;

    [SerializeField]
    GridObjectFactory factory;

    [SerializeField]
    GridConverter gridConverter;

    [SerializeField]
    ItemRuntimeSet itemRuntimeSet;

    List<SortingGraphNode> nodes = new List<SortingGraphNode>();

    List<SortingGraphNode> topologicalOrdering = new List<SortingGraphNode>();

    void Awake()
    {
        factory.onItemCreated += CreateSortingNode;
        placementManager.OnItemMoved += UpdateGraphForItemMoved;
    }

    void Start()
    {
        // By Start(), every object's Awake() has already run — including
        // whatever populated the runtime set. Safe to snapshot now.
        foreach (Item existingItem in itemRuntimeSet.Items)
        {
            if (existingItem.gameObject.GetComponent<SortingGraphNode>() == null)
            {
                CreateSortingNode(existingItem);
            }
        }
    }

    void OnDestroy()
    {
        factory.onItemCreated -= CreateSortingNode;
        placementManager.OnItemMoved -= UpdateGraphForItemMoved;
    }

    void CreateSortingNode(Item item)
    {
        SortingGraphNode node = item.gameObject.AddComponent<SortingGraphNode>();

        UpdateOrderingWithinBounds(node.GetSpRendererBounds());
        nodes.Add(node);

        UpdateOrdering();
    }

    void UpdateOrderingWithinBounds(Bounds bounds)
    {

        List<SortingGraphNode> overlaps = Physics2D.OverlapAreaAll(bounds.min, bounds.max)
        .Select(collider => collider.gameObject.GetComponent<SortingGraphNode>())
        .Where(n => n != null)
        .ToList();


        foreach (var item in overlaps)
        {
            Bounds otherBounds = item.GetSpRendererBounds();
            List<SortingGraphNode> otherItemOverlaps = Physics2D.OverlapAreaAll(otherBounds.min, otherBounds.max)
            .Select(collider => collider.gameObject.GetComponent<SortingGraphNode>())
            .Where(n => n != null)
            .ToList();
            Debug.Log("updating" + item.name + " has " + otherItemOverlaps.Count + " overlaps under it.");
            item.UpdateEdgesWithOverlaps(otherItemOverlaps);
        }
    }

    void UpdateGraphForItemMoved(Item item, Vector2Int oldPosition, Vector2Int newPosition)
    {
        SortingGraphNode itemNode = item.gameObject.GetComponent<SortingGraphNode>();
        Bounds newBounds = itemNode.GetSpRendererBounds();


        Bounds oldBounds = newBounds;
        oldBounds.center += gridConverter.GridCoordsToWorldCoords(oldPosition) - gridConverter.GridCoordsToWorldCoords(newPosition);

        UpdateOrderingWithinBounds(oldBounds);
        UpdateOrderingWithinBounds(newBounds);

        UpdateOrdering();
        Debug.Log("this should be called after all the comparisons");
    }



    private void AssignTopologicalSortingOrder()
    {
        for (int i = 0; i < topologicalOrdering.Count; i++)
        {
            topologicalOrdering[i].SetSortOrderOfNode(i + 100);//TODO: move to some const, adding +100 just so we dont start from 0

        }
    }

    private void UpdateOrdering()
    {
        topologicalOrdering = SortTopologically();
        AssignTopologicalSortingOrder();
    }

    private List<SortingGraphNode> SortTopologically()
    {
        Stack<SortingGraphNode> stack = new Stack<SortingGraphNode>();
        HashSet<SortingGraphNode> visited = new HashSet<SortingGraphNode>();
        HashSet<SortingGraphNode> inProgress = new HashSet<SortingGraphNode>();

        for (int i = 0; i < nodes.Count; i++)
        {
            VisitNode(nodes[i], visited, inProgress, stack);
        }

        return stack.Reverse().ToList();
    }

    private void VisitNode(SortingGraphNode node, HashSet<SortingGraphNode> visited, HashSet<SortingGraphNode> inProgress, Stack<SortingGraphNode> stack)
    {
        if (visited.Contains(node))
        {
            return;
        }

        if (inProgress.Contains(node))
        {
            Debug.LogWarning("Cycle detected in sorting graph at node: " + node.gameObject.name);
            return;
        }

        inProgress.Add(node);

        visited.Add(node);

        foreach (SortingGraphNode edge in node.edges)
        {
            VisitNode(edge, visited, inProgress, stack);
        }

        stack.Push(node);
    }

}
