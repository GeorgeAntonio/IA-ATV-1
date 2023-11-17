using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Patrol : Node
{
    private Controller controller;
    private List<Vector2> path;
    private int currentPathIndex;

    public Patrol(Controller controller)
    {
        this.controller = controller;
        this.path = new List<Vector2>();
        this.currentPathIndex = 0;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.ToString());

        if (!controller.IsStunned())
        {
            if (controller.patrolPoint != null)
            {
                // Check if the NPC has reached the current patrol point
                if (Vector2.Distance(controller.transform.position, controller.patrolPoint.position) <= controller.patrolCircleRadius)
                {
                    // Change patrol point when reached or within a certain range
                    controller.patrolPoint = controller.GetRandomPatrolPoint();

                    // Calculate a new path to the patrol point using A* algorithm
                    path = AStar(controller.transform.position, controller.patrolPoint.position);
                    currentPathIndex = 0;
                }

                // Move towards the next position in the path
                if (currentPathIndex < path.Count)
                {
                    Vector2 nextPosition = path[currentPathIndex];
                    Vector2 patrolDirection = (nextPosition - (Vector2)controller.transform.position).normalized;

                    controller.moveDirection = patrolDirection;
                    controller.rb.velocity = controller.moveDirection * controller.moveSpeed;

                    // Check if the NPC is close enough to the next position in the path
                    if (Vector2.Distance(controller.transform.position, nextPosition) < 0.1f)
                    {
                        currentPathIndex++;
                    }
                }
            }
            else
            {
                // No patrol point set, stop patrolling
                controller.rb.velocity = Vector2.zero;
            }
        }

        return NodeState.RUNNING;
    }

    // A* pathfinding algorithm
    private List<Vector2> AStar(Vector2 start, Vector2 goal)
    {
        // Nodes to be evaluated
        List<Vector2> openSet = new List<Vector2>();
        // Nodes already evaluated
        HashSet<Vector2> closedSet = new HashSet<Vector2>();
        // Cost from start along best known path
        Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float>();
        // Estimated total cost from start to goal through y
        Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float>();
        // Parent nodes
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Vector2.Distance(start, goal);

        while (openSet.Count > 0)
        {
            Vector2 current = GetLowestFScore(openSet, fScore);
            openSet.Remove(current);

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Vector2.Distance(current, neighbor);

                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Vector2.Distance(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return new List<Vector2>();
    }

    // Helper method to get the neighbor nodes
    private List<Vector2> GetNeighbors(Vector2 position)
    {
        List<Vector2> neighbors = new List<Vector2>();

        Vector2 up = position + Vector2.up;
        Vector2 down = position + Vector2.down;
        Vector2 left = position + Vector2.left;
        Vector2 right = position + Vector2.right;

        if (!IsWall(up))
            neighbors.Add(up);

        if (!IsWall(down))
            neighbors.Add(down);

        if (!IsWall(left))
            neighbors.Add(left);

        if (!IsWall(right))
            neighbors.Add(right);

        return neighbors;
    }

    // Helper method to check if a position is a wall
    private bool IsWall(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Wall"))
            {
                return true;
            }
        }

        return false;
    }

    // Helper method to get the lowest fScore node from the open set
    private Vector2 GetLowestFScore(List<Vector2> openSet, Dictionary<Vector2, float> fScore)
    {
        float lowestFScore = float.MaxValue;
        Vector2 lowestNode = Vector2.zero;

        foreach (Vector2 node in openSet)
        {
            if (fScore.TryGetValue(node, out float nodeFScore) && nodeFScore < lowestFScore)
            {
                lowestFScore = nodeFScore;
                lowestNode = node;
            }
        }

        return lowestNode;
    }

    // Helper method to reconstruct the path from the cameFrom dictionary
    private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        List<Vector2> path = new List<Vector2>();
        path.Add(current);

        while (cameFrom.TryGetValue(current, out Vector2 previous))
        {
            current = previous;
            path.Add(current);
        }

        path.Reverse();
        return path;
    }
}
