using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapInterfaceController : MonoBehaviour
{
    [SerializeField] private Tile _highlightTile;
    [SerializeField] private Tilemap _interfaceTilemap;

    private List<Node> _currentPath = new List<Node>();

    public void ShowPath(List<Node> path)
    {
        if (_currentPath != null)
            foreach (Node tile in _currentPath)
                _interfaceTilemap.SetTile(tile.TilemapPosition, null);

        _currentPath = path;

        if (_currentPath == null)
            return;

        for (int loop = 0; loop < _currentPath.Count; loop++)
        {
            _interfaceTilemap.SetTile(_currentPath[loop].TilemapPosition, _highlightTile);
            _interfaceTilemap.SetTileFlags(_currentPath[loop].TilemapPosition, TileFlags.None);
            _interfaceTilemap.SetColor(_currentPath[loop].TilemapPosition, loop == _currentPath.Count - 1? Color.blue : Color.white);
        }
    }

    public void CleanTile(Vector3Int tilePosition)
    {
        _interfaceTilemap.SetTile(tilePosition, null);
    }

    public void OnClickCreateGroup()
    {
        CreateGroupWindow createGroupWindow = WindowManager.Instance.GetWindow<CreateGroupWindow>();
        createGroupWindow.onGroupReady = (Dictionary<int, int> newGroup) =>
        {
            createGroupWindow.Hide();

            GameController.Instance.PlayerData.SetGroup(newGroup);
        };
        createGroupWindow.Show();
    }
}
