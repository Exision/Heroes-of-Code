using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float _speed = 20;

    private List<Node> _path;
    private int _targetIndex;
    private Coroutine _followingPathRoutine;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            PathfindingManager.RequestPath(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), OnPathFound);
    }

    private void OnDrawGizmos()
    {
        if (_path != null)
        {
            for (int loop = _targetIndex; loop < _path.Count; loop++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(_path[loop].WorldPosition, Vector3.one * 0.2f);

                if (loop == _targetIndex)
                    Gizmos.DrawLine(transform.position, _path[loop].WorldPosition);
                else
                    Gizmos.DrawLine(_path[loop - 1].WorldPosition, _path[loop].WorldPosition);
            }
        }
    }

    private void OnPathFound(List<Node> path)
    {
        if (path != null)
        {
            if (_followingPathRoutine == null)
            {
                this._path = path;

                _followingPathRoutine = StartCoroutine(FollowPath());
            }
        }
    }

    private IEnumerator FollowPath()
    {
        Node currentWaypoint = _path[0];

        while(true)
        {
            if (transform.position == currentWaypoint.WorldPosition)
            {
                _targetIndex++;

                if (_targetIndex >= _path.Count)
                {
                    _followingPathRoutine = null;
                    _path = null;
                    _targetIndex = default;

                    yield break;
                }

                currentWaypoint = _path[_targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.WorldPosition, _speed * Time.deltaTime);

            yield return null;
        }
    }
}
