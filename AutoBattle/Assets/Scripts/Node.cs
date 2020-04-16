using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;
    public int movementPenalty;

    public int g_cost;
    public int h_cost;
    public Node parent;
    int heapIndex;

    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY, int movementPenalty)
    {
        this.walkable = walkable;
        this.worldPos = worldPos;
        this.gridX = gridX;
        this.gridY = gridY;
        this.movementPenalty = movementPenalty;
    }

    public int f_cost
    {
        get{
            return g_cost + h_cost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = f_cost.CompareTo(nodeToCompare.f_cost);
        if(compare == 0)
        {
            compare = h_cost.CompareTo(nodeToCompare.h_cost);
        }
        return -compare;
    }
}
