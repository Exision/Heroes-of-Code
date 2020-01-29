using System;
using System.Collections.Generic;
using UnityEngine;

public class MapChip : MonoBehaviour
{
    public List<Node> Path { get; protected set; }

    protected int _nextNodeIndex;


    public virtual void Move()
    {
        MoveNextTile();
    }

    private void MoveNextTile()
    {
        if (Path == null || Path.Count <= 0)
            return;

        transform.position = Vector3.MoveTowards(transform.position, Path[_nextNodeIndex].WorldPosition, GameConfig.Instance.unitSpeed);

        _nextNodeIndex++;

        if (_nextNodeIndex >= Path.Count)
        {
            Path = null;
            _nextNodeIndex = default;

            return;
        }
    }

    public bool IsNeighbour(Vector3 checkPosition)
    {
        return Mathf.Abs(transform.position.x - checkPosition.x) <= 1 && Mathf.Abs(transform.position.y - checkPosition.y) <= 1;
    }
}
