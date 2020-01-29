using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : SingletonMonoBehaviour<MapController>
{
    [SerializeField] private PlayerMapChip _player;
    [SerializeField] private InterfaceController _interfaceController;
    [SerializeField] private MapChipManager _unitsController;

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
        ChangeGameState(E_MapGameState.Waiting);
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

            _unitsController.TryMoveUnits();
            
            if (_unitsController.IsNeighbour(_player.transform.position))
            {
                ChangeGameState(E_MapGameState.Waiting);

                StartFight();

                yield break;
            }

            yield return _coroutineHalfSecond;
        }

        ChangeGameState(E_MapGameState.Waiting);

        yield break;
    }

    private void StartFight()
    {

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

                                _interfaceController.ShowPath(path);
                            }
                        });

                    break;
                }
        }
    }

    private void OnPlayerMoveDone(Vector3Int tilePosition)
    {
        _interfaceController.CleanTile(tilePosition);
    }
}
