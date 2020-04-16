﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path 
{
    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;

    public Path(Vector3[] waypoints, Vector3 startPos, float turnDst, float stoppingDst)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 previousPoint = ConverV3ToV2(startPos);
        for(int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currPoint = ConverV3ToV2(lookPoints[i]);
            Vector2 dirToCurrPoint = (currPoint-previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i== finishLineIndex)?currPoint : currPoint-dirToCurrPoint *turnDst;
            turnBoundaries[i] = new Line(turnBoundaryPoint,previousPoint-dirToCurrPoint*turnDst);
            previousPoint = turnBoundaryPoint;
        }

        float distanceFromEndPoint = 0;
        for(int i = lookPoints.Length - 1; i > 0; i--)
        {
            distanceFromEndPoint += Vector3.Distance(lookPoints[i], lookPoints[i - 1]);
            if(distanceFromEndPoint > stoppingDst)
            {
                slowDownIndex = i;
                break;
            }
        }
    }

    Vector2 ConverV3ToV2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public void DrawWithGizmos(Vector3 player)
    {
       // Vector3 temp= player;
        Gizmos.color = Color.black;
        foreach(Vector3 p in lookPoints)
        {
            Gizmos.DrawCube(p+Vector3.up,Vector3.one);
           // Gizmos.DrawLine(p, temp);
           // temp = p;
        }
        Gizmos.color = Color.white;
        foreach(Line l in turnBoundaries)
        {
            l.DrawWithGizmos(2);
        }
    }
}
