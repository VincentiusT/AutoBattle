using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    //Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    //PathRequest currentPathRequest;
    //bool isProcessingPath=false;

    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    PathFinding pathFinding;


    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    private void Update()
    {
        if (results.Count > 0)
        {
            int itemInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(/*Vector3 startPath, Vector3 endPath, Action<Vector3[], bool> callback*/ PathRequest request)
    {
        //PathRequest newRequest = new PathRequest(startPath, endPath, callback);
        //instance.pathRequestQueue.Enqueue(newRequest);
        //instance.TryProcessingNext();

        ThreadStart threadStart = delegate
        {
            instance.pathFinding.findPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(/*Vector3[] path, bool success, PathRequest originalRequest*/ PathResult result)
    {
        //currentPathRequest.callback(path,success);
        //isProcessingPath = false;
        //TryProcessingNext();
        
        lock (results)
        {
            results.Enqueue(result);
        }
    }

    
    //void TryProcessingNext()
    //{
    //    if(!isProcessingPath && pathRequestQueue.Count >0)
    //    {
    //        currentPathRequest = pathRequestQueue.Dequeue();
    //        isProcessingPath = true;
    //        pathFinding.startFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
    //    }
    //}
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}


public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[],bool> _callback)
    {
        pathStart = _pathStart;
        pathEnd = _pathEnd;
        callback = _callback;
    }
}
