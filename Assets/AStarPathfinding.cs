using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    // Nó representa uma célula no grid
    private class Node
    {
        public Vector3Int position;
        public int gCost; // Custo do início até este nó
        public int hCost; // Custo heurístico deste nó até o destino
        public Node parent;

        public int FCost => gCost + hCost; // Custo total (F = G + H)

        public Node(Vector3Int pos)
        {
            position = pos;
        }
    }

    public List<Vector3Int> FindPath(Vector3Int startCell, Vector3Int targetCell, Grid grid)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node startNode = new Node(startCell);
        Node targetNode = new Node(targetCell);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.position == targetNode.position)
            {
                path = RetracePath(startNode, targetNode);
                return path;
            }

            foreach (Vector3Int neighborPos in GetNeighbors(currentNode.position, grid))
            {
                Node neighbor = new Node(neighborPos);
                if (!IsCellWalkable(neighborPos, grid) || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return path;
    }

    private List<Vector3Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
        int dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

        // Use o custo diagonal de 14 e o custo vertical/horizontal de 10
        return 10 * (dstX + dstY) + (14 - 2 * 10) * Mathf.Min(dstX, dstY);
    }

    private List<Vector3Int> GetNeighbors(Vector3Int cell, Grid grid)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                Vector3Int neighborPos = new Vector3Int(cell.x + x, cell.y + y, cell.z);
                neighbors.Add(neighborPos);
            }
        }

        return neighbors;
    }

    private bool IsCellWalkable(Vector3Int cell, Grid grid)
    {
        // Adapte isso de acordo com suas condições específicas
        return true;
    }
}
