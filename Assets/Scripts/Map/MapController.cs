using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : SingletonMonoBehaviour<MapController>
{
    [SerializeField] private PlayerMapChip _player;
    [SerializeField] private Grid _grid;
    [SerializeField] private MapInterfaceController _mapInterfaceController;
    [SerializeField] private MapChipManager _mapChipController;

    public E_MapGameState CurrentGameState { get; private set; }

    private WaitForSeconds _coroutineHalfSecond = new WaitForSeconds(0.5f);

    private void OnEnable()
    {
        InputController.onPositionSelected += OnPositionSelected;
        _player.onMoveDone += OnPlayerMoveDone;
    }

    private void OnDisable()
    {
        InputController.onPositionSelected -= OnPositionSelected;
        _player.onMoveDone -= OnPlayerMoveDone;
    }

    private void Start()
    {
        GenerateEnemysMapChips();

        ChangeGameState(E_MapGameState.Waiting);
    }

    private void GenerateEnemysMapChips()
    {
        Dictionary<int, Vector3> enemysPositions = new Dictionary<int, Vector3>();
        int gridX = default;
        int gridY = default;

        foreach (KeyValuePair<int, List<Troop>> enemy in GameController.Instance.EnemysDatas)
        {
            do
            {
                gridX = UnityEngine.Random.Range(5, _grid.MapGrid.GetLength(0));
                gridY = UnityEngine.Random.Range(5, _grid.MapGrid.GetLength(1));
            }
            while (!_grid.MapGrid[gridX, gridY].Walkable && !enemysPositions.ContainsValue(_grid.MapGrid[gridX, gridY].WorldPosition));

            enemysPositions.Add(enemy.Key, _grid.MapGrid[gridX, gridY].WorldPosition);
        }

        _mapChipController.Init(enemysPositions);
    }

    private void ChangeGameState(E_MapGameState newGameState)
    {
        CurrentGameState = newGameState;

        switch (CurrentGameState)
        {
            case E_MapGameState.Moving:
                StartCoroutine(UnitsMoving());
                break;
            case E_MapGameState.Waiting:

                break;
        }
    }

    private IEnumerator UnitsMoving()
    {
        while (_player.Path != null)
        {
            _player.Move();

            _mapChipController.TryMoveUnits();
            
            if (_mapChipController.IsNeighbour(_player.transform.position, out int enemyId))
            {
                ChangeGameState(E_MapGameState.Waiting);

                GameController.Instance.StartFight(enemyId);

                yield break;
            }

            if (_player.Path == null)
                break;

            yield return _coroutineHalfSecond;
        }

        _mapInterfaceController.CleanPath();

        ChangeGameState(E_MapGameState.Waiting);

        yield break;
    }

    private void OnPositionSelected(Vector3 worldPosition)
    {
        switch (CurrentGameState)
        {
            case E_MapGameState.Waiting:
                {
                    PathfindingManager.RequestPath(_player.transform.position,
                        worldPosition,
                        (List<Node> resultPath) =>
                        {
                            List<Node> path = resultPath;

                            if (_player.Path != null && path != null && System.Linq.Enumerable.SequenceEqual(_player.Path, path))
                                ChangeGameState(E_MapGameState.Moving);
                            else
                            {
                                _player.SetPath(path);

                                _mapInterfaceController.ShowPath(path);
                            }
                        });

                    break;
                }
        }
    }

    private void OnPlayerMoveDone(Vector3Int tilePosition)
    {
        _mapInterfaceController.CleanTile(tilePosition);
    }
}
