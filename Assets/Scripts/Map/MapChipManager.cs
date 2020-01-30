using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChipManager : MonoBehaviour
{
    [SerializeField] private EnemyMapChip _enemyChipPrefab;

    private Dictionary<int, EnemyMapChip> _enemysDatas = new Dictionary<int, EnemyMapChip>();

    public void Init(Dictionary<int,Vector3> enemysPosition)
    {
        foreach(KeyValuePair<int, Vector3> enemy in enemysPosition)
        {
            EnemyMapChip enemyChip = Instantiate<EnemyMapChip>(_enemyChipPrefab, _enemyChipPrefab.transform.parent);
            enemyChip.transform.position = enemy.Value;

            _enemysDatas.Add(enemy.Key, enemyChip);
        }

        Destroy(_enemyChipPrefab.gameObject);
    }

    public void TryMoveUnits()
    {
        foreach (KeyValuePair<int, EnemyMapChip> enemy in _enemysDatas)
        {
            if (enemy.Value.Path == null && GameConfig.Instance.unitChanceToFindPath >= Random.value)
                enemy.Value.RequestRandomPath();
            else if (enemy.Value.Path != null)
                enemy.Value.Move();
        }
    }

    public bool IsNeighbour(Vector3 position, out int enemyId)
    {
        foreach (KeyValuePair<int, EnemyMapChip> enemy in _enemysDatas)
        {
            if (enemy.Value.IsNeighbour(position))
            {
                enemyId = enemy.Key;

                return true;
            }
        }

        enemyId = -1;

        return false;
    }
}
