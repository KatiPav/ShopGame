using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SortingGraph : MonoBehaviour
{
    [SerializeField]
    PlacementManager placementManager;

    [SerializeField]
    GridObjectFactory factory;

    List<SortingGraphNode> nodes = new List<SortingGraphNode>();

    List<SortingGraphNode> topologicalOrdering = new List<SortingGraphNode>();

    void Awake()
    {

        //grab all nodes and put them in the list

        factory.onItemCreated += CreateSortingNode;
        placementManager.OnItemMoved += UpdateSortingOrderWithItem;
    }

    void OnDestroy()
    {
        placementManager.OnItemMoved -= UpdateSortingOrderWithItem;
    }

    void CreateSortingNode(Item item)
    {
        SortingGraphNode node = item.gameObject.AddComponent<SortingGraphNode>();
        nodes.Add(node);
        UpdateOrdering();
    }

    void UpdateSortingOrderWithItem(Item item)
    {
        //i need to remove all the old references to this node 
        SortingGraphNode itemNode = item.gameObject.GetComponent<SortingGraphNode>();
        foreach (var overlapNode in itemNode.GetOverlappingNodes())
        {
            overlapNode.RemoveFromEdges(itemNode);
            overlapNode.UpdateEdges();
        }
        itemNode.UpdateEdges();




        //temporary
        topologicalOrdering = SortTopologically();
        AssignTopologicalSortingOrder();
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

        for (int i = 0; i < nodes.Count; i++)
        {
            VisitNode(nodes[i], visited, stack);
        }
        return stack.ToList();
    }

    private void VisitNode(SortingGraphNode node, HashSet<SortingGraphNode> visited, Stack<SortingGraphNode> stack)
    {
        if (visited.Contains(node))
        {
            return;
        }

        visited.Add(node);

        foreach (SortingGraphNode edge in node.edges)
        {
            VisitNode(edge, visited, stack);
        }

        stack.Push(node);
    }

}
