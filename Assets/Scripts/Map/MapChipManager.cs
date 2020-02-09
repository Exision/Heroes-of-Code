using System.Collections.Generic;
using UnityEngine;

public class MapChipManager : MonoBehaviour
{
    [SerializeField] private EnemyMapChip _enemyChipPrefab;

    private Dictionary<int, EnemyMapChip> _enemysDatas = new Dictionary<int, EnemyMapChip>();

    public void Init()
    {
        _enemysDatas.Clear();

        foreach (EnemyData enemy in GameController.Instance.EnemysDatas)
        {
            if (GameController.Instance.DefeatedEnemys.Contains(enemy))
                continue;

            EnemyMapChip enemyChip = Instantiate<EnemyMapChip>(_enemyChipPrefab, _enemyChipPrefab.transform.parent);
            enemyChip.transform.position = (Vector3)enemy.Position;

            _enemysDatas.Add(enemy.Id, enemyChip);
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
