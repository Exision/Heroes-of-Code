using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleController : MonoBehaviour
{
    private Dictionary<Troop, TroopObject> _playerGroup = new Dictionary<Troop, TroopObject>();

    private Dictionary<Troop, TroopObject> _enemyGroup = new Dictionary<Troop, TroopObject>();

    private List<BattleQueueElement> _battleQueue = new List<BattleQueueElement>();

    private E_BattleState _battleState = E_BattleState.Waiting;

    private void Start()
    {
        InitBattle();

        ChangeState(E_BattleState.PlayerTurn);
    }

    private void InitBattle()
    {
        InitPlayer();
        InitEnemys();

        CreateQueue();
    }

    private void InitEnemys()
    {
        List<Troop> enemyTroops = GameController.Instance.EnemysDatas[GameController.Instance.CurrentEnemy];

        TroopObject[] enemyObjects = new EnemyFactory().Create(enemyTroops);

        for (int loop = 0; loop < enemyTroops.Count; loop++)
        {
            _enemyGroup.Add(enemyTroops[loop], enemyObjects[loop]);
        }
    }

    private void InitPlayer()
    {
        TroopObject[] playerObjects = new PlayerFactory().Create(GameController.Instance.PlayerData.Group);

        for (int loop = 0; loop < playerObjects.Length; loop++)
        {
            _playerGroup.Add(GameController.Instance.PlayerData.Group[loop], playerObjects[loop]);
        }
    }

    private void CreateQueue()
    {
        for (int loop = 0; loop < Mathf.Max(_playerGroup.Count, _enemyGroup.Count); loop++)
        {
            if (loop < _playerGroup.Count)
            {
                BattleQueueElement queueElement = new BattleQueueElement(_playerGroup.ElementAt(loop).Key, _playerGroup.ElementAt(loop).Value, BattleQueueElement.E_ElementTeam.Player);

                _battleQueue.Add(queueElement);
            }
            else
            {
                BattleQueueElement queueElement = new BattleQueueElement(_playerGroup.ElementAt(loop % _playerGroup.Count).Key, _playerGroup.ElementAt(loop % _playerGroup.Count).Value, BattleQueueElement.E_ElementTeam.Player);

                _battleQueue.Add(queueElement);
            }

            if (loop < _enemyGroup.Count)
            {
                BattleQueueElement queueElement = new BattleQueueElement(_enemyGroup.ElementAt(loop).Key, _enemyGroup.ElementAt(loop).Value, BattleQueueElement.E_ElementTeam.Enemy);

                _battleQueue.Add(queueElement);
            }
            else
            {
                BattleQueueElement queueElement = new BattleQueueElement(_enemyGroup.ElementAt(loop % _enemyGroup.Count).Key, _enemyGroup.ElementAt(loop % _enemyGroup.Count).Value, BattleQueueElement.E_ElementTeam.Enemy);

                _battleQueue.Add(queueElement);
            }
        }

        Debug.Log("Queue lenght " + _battleQueue.Count);
    }

    private void ChangeState(E_BattleState state)
    {

    }
}
