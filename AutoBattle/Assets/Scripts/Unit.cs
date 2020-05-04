using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    const float pathUpdateMoveThreshold = 0.5f;
    const float minPathUpdateTime = 0.2f;
    protected Transform target;
    protected float speed = 0f;
    public float turnDst = 5;
    public float turnSpeed = 3;
    public float stoppingDst = 10;
    public bool isDrawPath;

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
        if (transform == null) return;
        if (pathSuccess && transform!=null)
        {
             //path = newPath;
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
       if(transform!=null)
            PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound, unitPos));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetOldPos = target.position;
        while (isKeepUpdatetingPath)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if (target != null)
            {
                if((target.position - targetOldPos).sqrMagnitude > sqrMoveThreshold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound, unitPos));
                    targetOldPos = target.position;
                }
            }
        }
    }

    IEnumerator followPath()
    {
        //Vector3 currentWayPoint = path[0];// --kode lama sebelum pake class path

        //while (true)
        //{
        //    if (transform.position == currentWayPoint)
        //    {
        //        targetIndex++;
        //        if (targetIndex >= path.Length) yield break;

        //        currentWayPoint = path[targetIndex];
        //    }
        //    transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
        //    yield return null;
        //}
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

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
                Quaternion targetRotaion = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotaion, Time.deltaTime * turnSpeed);
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
            //for (int i = targetIndex; i < path.lookPoints.Length; i++)
            //{
            //    Gizmos.color = Color.black;
            //    Gizmos.DrawCube(path.lookPoints[i], Vector3.one );

            //    if (i == targetIndex)
            //    {
            //        Gizmos.DrawLine(transform.position, path.lookPoints[i]);
            //    }
            //    else
            //    {
            //        Gizmos.DrawLine(path.lookPoints[i - 1], path.lookPoints[i]);
            //    }
            //}
        }
    }
}
