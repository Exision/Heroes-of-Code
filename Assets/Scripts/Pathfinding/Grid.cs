using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    [SerializeField] public Tilemap _tilemap;

    private Node[,] _grid;
    private BoundsInt _tilemapBounds;

    public int MaxSize
    {
        get
        {
            return _grid.GetLength(0) * _grid.GetLength(1);
        }
    }

    private void Awake()
    {
        _tilemap.CompressBounds();
        _tilemapBounds = _tilemap.cellBounds;

        Debug.Log($"Total size: {_tilemapBounds.size}, min position: {_tilemapBounds.min}, max position: {_tilemapBounds.max}");
            
        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Node[_tilemap.size.x, _tilemap.size.y];

        for (int x = _tilemapBounds.xMin; x < _tilemapBounds.xMax; x++)
        {
            for (int y = _tilemapBounds.yMin; y < _tilemapBounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                ScriptableTile tile = _tilemap.GetTile<ScriptableTile>(position);
                Node node = new Node(_tilemap.CellToWorld(position), new Vector2Int(x - _tilemapBounds.xMin, y - _tilemapBounds.yMin), tile.IsWalkable, tile.Penalty);

                _grid[node.GridPosition.x, node.GridPosition.y] = node;
            }
        }
    }

    public Node GetNodeByWorld(Vector3 worldPosition)
    {
        Vector3Int position = _tilemap.WorldToCell(worldPosition);

        int indexX = position.x - _tilemapBounds.xMin;
        int indexY = position.y - _tilemapBounds.yMin;

        if (indexX >= 0 && indexX < _grid.GetLength(0) && indexY >= 0 && indexY < _grid.GetLength(1))
            return _grid[position.x - _tilemapBounds.xMin, position.y - _tilemapBounds.yMin];
        else
            return null;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int neighbourX = node.GridPosition.x + x;
                int neighbourY = node.GridPosition.y + y;

                if (neighbourX >= 0 && neighbourX < _grid.GetLength(0) && neighbourY >= 0 && neighbourY < _grid.GetLength(1))
                    neighbours.Add(_grid[neighbourX, neighbourY]);
            }
        }

        return neighbours;
    }
}
