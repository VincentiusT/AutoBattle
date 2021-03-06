﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    const float pathUpdateMoveThreshold = 0.5f;
    const float minPathUpdateTime = 0.2f;
    protected Transform target;
    protected float speed = 0f;
    private float turnDst = 0.5f;
    private float turnSpeed = 10;
    private float stoppingDst = 10;
    public bool isDrawPath;
    protected bool isAttacking = false;

    public bool isKeepUpdatetingPath=true;
    Vector3 unitPos;
    Path path;

    //Vector3[] path;
    int targetIndex;


    private void Update()
    {
        unitPos = transform.position;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = new Path(newPath,transform.position,turnDst, stoppingDst);
            StopCoroutine("followPath");
            StartCoroutine("followPath");
        }
    }

    public IEnumerator updatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
            yield return new WaitForSeconds(0.3f);
        // PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        if(transform!=null && target != null)
        {

            PathRequest pr = new PathRequest(transform.position, target.position, OnPathFound);
            PathRequestManager.RequestPath(pr);
        }
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetOldPos=Vector3.zero;
        if (target != null)
            targetOldPos = target.position;
        while (isKeepUpdatetingPath)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if (target != null)
            {

                if((target.position - targetOldPos).sqrMagnitude > sqrMoveThreshold)
                {

                    PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                    targetOldPos = target.position;
                }
            }
        }
    }

    IEnumerator followPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        //transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (true)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                    pathIndex++;
            }

            if (followingPath)
            {
                if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                Quaternion targetRotationInY = new Quaternion(0,targetRotation.y,0,targetRotation.w); 
               if(!isAttacking)
                   transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationInY, Time.deltaTime * turnSpeed);

                //transform.rotation = targetRotationInY;
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }

            yield return null;
        }
    }

    protected Transform getClosestGameObject(Collider[] objects)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Collider t in objects)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    protected Transform getClosestGameObject(List<GameObject> objects)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in objects)
        {
            if (t != null)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }

    protected void OnDrawGizmos()
    {
        if(path != null && isDrawPath)
        {

            path.DrawWithGizmos(transform.position);

        }
    }

}
