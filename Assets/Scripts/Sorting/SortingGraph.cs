using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SortingGraph : MonoBehaviour
{
    [SerializeField]
    PlacementManager placementManager;

    List<SortingGraphNode> nodes = new List<SortingGraphNode>();

    List<SortingGraphNode> topologicalOrdering = new List<SortingGraphNode>();

    void Awake()
    {
        placementManager.OnItemMoved += UpdateSortingOrderWithItem;
    }

    void OnDestroy()
    {
        placementManager.OnItemMoved -= UpdateSortingOrderWithItem;
    }

    void UpdateSortingOrderWithItem(Item item)
    {
        //remove old item first ??
        SortingGraphNode node = new SortingGraphNode(item);
        nodes.Add(node);
        //UpdateTopologicalOrdering();

        //temporary
        SortTopologically();
        AssignTopologicalSortingOrder();
    }


    private void AssignTopologicalSortingOrder()
    {
        for (int i = 0; i < topologicalOrdering.Count; i++)
        {
            topologicalOrdering[i].SetSortOrderOfNode(i + 100);//TODO: move to some const, adding +100 just so we dont start from 0
        }
    }

    private void UpdateTopologicalOrdering()
    {

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
