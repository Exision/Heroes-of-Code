using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChipManager : MonoBehaviour
{
    [SerializeField] private MapChip[] _enemyUnits;

    public void TryMoveUnits()
    {
        foreach(EnemyMapChip unit in _enemyUnits)
        {
            if (unit.Path == null && GameConfig.Instance.unitChanceToFindPath >= Random.value)
                unit.RequestRandomPath();
            else if (unit.Path != null)
                unit.Move();
        }
    }

    public bool IsNeighbour(Vector3 position)
    {
        foreach (EnemyMapChip unit in _enemyUnits)
        {
            if (unit.IsNeighbour(position))
                return true;
        }

        return false;
    }
}
