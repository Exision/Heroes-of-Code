using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsController : MonoBehaviour
{
    [SerializeField] private Unit[] _enemyUnits;

    private PlayerUnit _player;


    private void OnDisable()
    {
        _player.onMoveDone -= TryMoveUnits;
    }

    public void Init(PlayerUnit player)
    {
        _player = player;

        _player.onMoveDone += TryMoveUnits;
    }

    public void TryMoveUnits(Vector3Int tilePosition)
    {
        foreach(EnemyUnit unit in _enemyUnits)
        {
            if (unit.Path == null && GameConfig.Instance.unitChanceToFindPath >= Random.value)
                unit.RequestRandomPath();
            else if (unit.Path != null)
                unit.Move();

            if (unit.IsNeighbour(_player.transform.position))
                Debug.Log("Attack!");
        }
    }
}
