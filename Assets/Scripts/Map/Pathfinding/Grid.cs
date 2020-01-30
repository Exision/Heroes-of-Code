using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    [SerializeField] public Tilemap _tilemap;
    private BoundsInt _tilemapBounds;

    public int MaxSize
    {
        get
        {
            return MapGrid.GetLength(0) * MapGrid.GetLength(1);
        }
    }

    public Node[,] MapGrid { get; private set; }

    private void Awake()
    {
        _tilemap.CompressBounds();
        _tilemapBounds = _tilemap.cellBounds;

        CreateGrid();
    }

    private void CreateGrid()
    {
        MapGrid = new Node[_tilemap.size.x, _tilemap.size.y];

        for (int x = _tilemapBounds.xMin; x < _tilemapBounds.xMax; x++)
        {
            for (int y = _tilemapBounds.yMin; y < _tilemapBounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                ScriptableTile tile = _tilemap.GetTile<ScriptableTile>(position);

                // Wrong world position. Add 0.5f
                Node node = new Node(_tilemap.CellToWorld(position) + new Vector3(0.5f, 0.5f),
                    position,
                    new Vector2Int(x - _tilemapBounds.xMin, y - _tilemapBounds.yMin),
                    tile.IsWalkable,
                    tile.Penalty);

                MapGrid[node.GridPosition.x, node.GridPosition.y] = node;
            }
        }
    }

    public Node GetNodeByWorld(Vector3 worldPosition)
    {
        Vector3Int position = _tilemap.WorldToCell(worldPosition);

        int indexX = position.x - _tilemapBounds.xMin;
        int indexY = position.y - _tilemapBounds.yMin;

        if (indexX >= 0 && indexX < MapGrid.GetLength(0) && indexY >= 0 && indexY < MapGrid.GetLength(1))
            return MapGrid[position.x - _tilemapBounds.xMin, position.y - _tilemapBounds.yMin];
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

                if (neighbourX >= 0 && neighbourX < MapGrid.GetLength(0) && neighbourY >= 0 && neighbourY < MapGrid.GetLength(1))
                    neighbours.Add(MapGrid[neighbourX, neighbourY]);
            }
        }

        return neighbours;
    }
}