using UnityEngine;

public class Node : IHeapItem<Node>
{
    #region Tilemap properties
    public bool Walkable { get; private set; }
    public int Penalty { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public Vector3Int TilemapPosition { get; private set; } 
    #endregion

    #region Grid properties
    public int gCost;
    public int hCost;

    public Vector2Int GridPosition { get; private set; }
    public Node Parent { get; set; }
    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    #endregion

    public int HeapIndex { get; set; }

    public Node(Vector3 worldPosition, Vector3Int tilemapPosition, Vector2Int gridPosition, bool isWalkable, int penalty)
    {
        WorldPosition = worldPosition;
        TilemapPosition = tilemapPosition;
        Walkable = isWalkable;
        Penalty = penalty;
        GridPosition = gridPosition;
    }

    public int CompareTo(Node comparableNode)
    {
        int compare = FCost.CompareTo(comparableNode.FCost);

        if (compare == 0)
            compare = hCost.CompareTo(comparableNode.hCost);

        return -compare;
    }
}
