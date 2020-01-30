using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public void FindPath(Vector3 startPosition, Vector3 targetPosition, Action<List<Node>> callback)
    {
        StartCoroutine(GetPath(startPosition, targetPosition, callback));
    }

    public void FindPath(Node startNode, Node endNode, Action<List<Node>> callback)
    {
        StartCoroutine(GetPath(startNode, endNode, callback));
    }

    private IEnumerator GetPath(Vector3 startPosition, Vector3 targetPosition, Action<List<Node>> callback)
    {
        yield return GetPath(_grid.GetNodeByWorld(startPosition), _grid.GetNodeByWorld(targetPosition), callback);
    }

    private IEnumerator GetPath(Node startNode, Node targetNode, Action<List<Node>> callback)
    {
        if (startNode == null || targetNode == null || !startNode.Walkable || !targetNode.Walkable)
        {
            callback?.Invoke(null);

            yield break;
        }

        Heap<Node> openNodes = new Heap<Node>(_grid.MaxSize);
        HashSet<Node> closeNodes = new HashSet<Node>();

        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes.RemoveFirst();
            closeNodes.Add(currentNode);

            if (currentNode == targetNode)
                break;

            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                if (!neighbour.Walkable || closeNodes.Contains(neighbour))
                    continue;

                int moveCost = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.Penalty;

                if (moveCost < neighbour.gCost || !openNodes.Contains(neighbour))
                {
                    neighbour.gCost = moveCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openNodes.Contains(neighbour))
                        openNodes.Add(neighbour);
                    else
                        openNodes.UpdateItem(neighbour);
                }
            }
        }

        yield return null;

        callback?.Invoke(RetracePath(startNode, targetNode));

        yield break;
    }

    private int GetDistance(Node fromNode, Node toNode)
    {
        int distanceX = Mathf.Abs(fromNode.GridPosition.x - toNode.GridPosition.x);
        int distanceY = Mathf.Abs(fromNode.GridPosition.y - toNode.GridPosition.y);

        return distanceX > distanceY ? 14 * distanceY + 10 * (distanceX - distanceY) :
            14 * distanceX + 10 * (distanceY - distanceX);
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.Parent;
        }

        path.Add(startNode);

        path.Reverse();
        return path;
    }
}