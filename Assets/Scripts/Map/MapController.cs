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
        MapInputController.onPositionSelected += OnPositionSelected;

        _player.onMoveDone += OnPlayerMoveDone;
    }

    private void OnDisable()
    {
        MapInputController.onPositionSelected -= OnPositionSelected;

        _player.onMoveDone -= OnPlayerMoveDone;
    }

    private void Start()
    {
        GenerateEnemysMapChips();

        _player.transform.position = GameController.Instance.PlayerData.Position;

        ChangeGameState(E_MapGameState.Waiting);
    }

    private void GenerateEnemysMapChips()
    {
        bool isPositionGenerated = true;

        for (int loop = 0; loop < GameController.Instance.EnemysDatas.Count; loop++)
            isPositionGenerated = isPositionGenerated && (GameController.Instance.EnemysDatas[loop].Position != null);

        if (isPositionGenerated)
        {
            _mapChipController.Init();

            return;
        }

        List<Vector3> occupiedPositions = new List<Vector3>();

        foreach (EnemyData enemy in GameController.Instance.EnemysDatas)
        {
            int gridX = default;
            int gridY = default;

            do
            {
                gridX = UnityEngine.Random.Range(5, _grid.MapGrid.GetLength(0));
                gridY = UnityEngine.Random.Range(5, _grid.MapGrid.GetLength(1));
            }
            while (!_grid.MapGrid[gridX, gridY].Walkable && !occupiedPositions.Contains(_grid.MapGrid[gridX, gridY].WorldPosition));

            enemy.Position = _grid.MapGrid[gridX, gridY].WorldPosition;
            occupiedPositions.Add(_grid.MapGrid[gridX, gridY].WorldPosition);
        }

        _mapChipController.Init();
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

    private void OnPlayerMoveDone(Vector3Int tilePosition, Vector3 worldPosition)
    {
        _mapInterfaceController.CleanTile(tilePosition);

        GameController.Instance.PlayerData.Position = worldPosition;
    }
}
