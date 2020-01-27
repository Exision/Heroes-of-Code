using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : SingletonMonoBehaviour<MapController>
{
    [SerializeField] private PlayerUnit _player;
    [SerializeField] private InterfaceController _interfaceController;
    [SerializeField] private UnitsController _unitsController;

    private E_MapGameState _currentGameState;
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

        _unitsController.Init(_player);
    }

    private void ChangeGameState(E_MapGameState newGameState)
    {
        _currentGameState = newGameState;

        switch (_currentGameState)
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

            yield return _coroutineHalfSecond;
        }

        ChangeGameState(E_MapGameState.Waiting);

        yield break;
    }

    private void OnPositionSelected(Vector3 worldPosition)
    {
        switch (_currentGameState)
        {
            case E_MapGameState.Waiting:
                {
                    List<Node> path = new List<Node>();

                    PathfindingManager.RequestPath(_player.transform.position,
                        worldPosition,
                        (List<Node> resultPath) =>
                        {
                            path = resultPath;

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
