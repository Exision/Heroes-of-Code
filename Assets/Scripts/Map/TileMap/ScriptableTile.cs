using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Scriptable Tile", menuName = "Tiles/Scriptable Tile")]
public class ScriptableTile : Tile
{
    [SerializeField] private bool _isWalkable;
    [SerializeField] private int _penalty;

    public bool IsWalkable { get => _isWalkable; }

    public int Penalty { get => _penalty; }
}