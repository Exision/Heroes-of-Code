using System;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : SingletonMonoBehaviour<PathfindingManager>
{
    private Queue<PathRequest> _pathRequestsQueue = new Queue<PathRequest>();
    private PathRequest _activePathRequest;
    private bool _isCalculatingPath;

    public static void RequestPath(Vector3 startPosition, Vector3 targetPosition, Action<List<Node>> callback)
    {
        PathRequest request = new PathRequest(startPosition, targetPosition, callback);

        Instance._pathRequestsQueue.Enqueue(request);
        Instance.TryStartNext();
    }

    private void TryStartNext()
    {
        if (!_isCalculatingPath && _pathRequestsQueue.Count > 0)
        {
            _activePathRequest = _pathRequestsQueue.Dequeue();
            _isCalculatingPath = true;

            Pathfinding.Instance.FindPath(_activePathRequest.startPosition, _activePathRequest.targetPosition, OnCalculationFinished);
        }
    }

    private void OnCalculationFinished(List<Node> path)
    {
        _activePathRequest.callback?.Invoke(path);
        _isCalculatingPath = false;

        TryStartNext();
    }

    private struct PathRequest
    {
        public Vector3 startPosition;
        public Vector3 targetPosition;
        public Action<List<Node>> callback;

        public PathRequest(Vector3 startPosition, Vector3 targetPosition, Action<List<Node>> callback)
        {
            this.startPosition = startPosition;
            this.targetPosition = targetPosition;
            this.callback = callback;
        }
    }
}
