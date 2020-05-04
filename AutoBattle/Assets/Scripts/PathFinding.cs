using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinding : MonoBehaviour
{
    //PathRequestManager requestManager;
    private GridMaker grid;

    private void Awake()
    {
        //requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<GridMaker>();
    }
  

    public void findPath(PathRequest request, Action<PathResult> callback)
    {

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.nodeFromWorldPoint(request.pathStart);
        Node targetNode = grid.nodeFromWorldPoint(request.pathEnd);
        Node unitNode = grid.nodeFromWorldPoint(request.unitNode);
        if (startNode.walkable && startNode.walkable || (!startNode.walkable && targetNode.isTower))
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closeSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.removeFirst();
                closeSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.getNeighbours(currentNode))
                {
                    if (!neighbour.walkable || (!neighbour.isTower && neighbour!=targetNode)||closeSet.Contains(neighbour)) continue;

                    int newMovementCostToNeighbour = currentNode.g_cost + getDistance(currentNode, neighbour) + neighbour.movementPenalty;
                    if (newMovementCostToNeighbour < neighbour.g_cost || !openSet.Contains(neighbour))
                    {
                        neighbour.g_cost = newMovementCostToNeighbour;
                        neighbour.h_cost = getDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                        else openSet.updateItem(neighbour);
                    }
                }
            }
        }

        if (pathSuccess)
        {
            wayPoints = retracePath(startNode, targetNode);
            pathSuccess = wayPoints.Length > 0;
        }
        callback(new PathResult(wayPoints, pathSuccess, request.callback));
    }

    private int getDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);  
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    private Vector3[] retracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = simplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] simplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if(newDirection != oldDirection)
            {
                waypoints.Add(path[i].worldPos);
            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
    }
}
