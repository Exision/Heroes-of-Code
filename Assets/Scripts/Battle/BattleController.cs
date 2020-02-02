using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private BattleInterfaceController _battleInterface;
    [SerializeField] private Camera currCamera;

    public Action<int> onCurrentElementChanged;
    public Action<string> onActionPerformed;

    private List<BattleQueueElement> _playerGroup = new List<BattleQueueElement>();

    private List<BattleQueueElement> _enemyGroup = new List<BattleQueueElement>();

    public List<BattleQueueElement> BattleQueue { get; private set; } = new List<BattleQueueElement>();

    private E_BattleState _battleState = E_BattleState.Waiting;

    private int _currentQueueElement = 0;
    private int _selectedSkill = -1;
    private List<BattleQueueElement> _selectedTroops = new List<BattleQueueElement>();

    private void Start()
    {
        InitBattle();

        SelectNextElement();
    }

    private void Update()
    {
        if (_battleState == E_BattleState.PlayerTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(currCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);

                Debug.Log(hits.Length);

                for (int loop = 0; loop < hits.Length; loop++)
                {
                    TroopObject troopObject = hits[loop].collider.gameObject.GetComponent<TroopObject>();

                    if (troopObject != null)
                        troopObject.OnClickTroop();
                }
            }
        }
    }

    private void InitBattle()
    {
        InitPlayer();
        InitEnemys();

        CreateQueue();

        _battleInterface.Init(this);
    }

    private void InitEnemys()
    {
        List<Troop> enemyTroops = GameController.Instance.EnemysDatas[GameController.Instance.CurrentEnemy];

        TroopObject[] enemyObjects = new EnemyFactory().Create(enemyTroops);

        for (int loop = 0; loop < enemyTroops.Count; loop++)
        {
            BattleQueueElement queueElement = new BattleQueueElement(enemyTroops[loop], enemyObjects[loop], BattleQueueElement.E_ElementTeam.Enemy);
            queueElement.onSkillUsed = OnSkillUsed;
            queueElement.onClickTroop = OnClickTroop;
            queueElement.onTroopDied = OnTroopDied;

            _enemyGroup.Add(queueElement);
        }
    }

    private void InitPlayer()
    {
        TroopObject[] playerObjects = new PlayerFactory().Create(GameController.Instance.PlayerData.Group);

        for (int loop = 0; loop < playerObjects.Length; loop++)
        {
            BattleQueueElement queueElement = new BattleQueueElement(GameController.Instance.PlayerData.Group[loop], playerObjects[loop], BattleQueueElement.E_ElementTeam.Player);
            queueElement.onSkillUsed = OnSkillUsed;
            queueElement.onClickTroop = OnClickTroop;
            queueElement.onTroopDied = OnTroopDied;

            _playerGroup.Add(queueElement);
        }
    }

    private void CreateQueue()
    {
        for (int loop = 0; loop < GameConfig.FindMultiple(_playerGroup.Count, _enemyGroup.Count); loop++)
        {
            BattleQueue.Add(loop < _playerGroup.Count ? _playerGroup[loop] : _playerGroup[loop % _playerGroup.Count]);

            BattleQueue.Add(loop < _enemyGroup.Count ? _enemyGroup[loop] : _enemyGroup[loop % _enemyGroup.Count]);
        }

        Debug.Log($"Plyer group: {_playerGroup.Count}, Enem: {_enemyGroup.Count} Queue lenght :{BattleQueue.Count}");
    }

    private void SelectNextElement()
    {
        //_currentQueueElement++;

        if (_currentQueueElement >= BattleQueue.Count)
            _currentQueueElement = 0;

        onCurrentElementChanged?.Invoke(_currentQueueElement);

        switch (BattleQueue[_currentQueueElement].Team)
        {
            case BattleQueueElement.E_ElementTeam.Player:
                ChangeState(E_BattleState.PlayerTurn);
                break;
            case BattleQueueElement.E_ElementTeam.Enemy:
                ChangeState(E_BattleState.EnemyTurn);
                break;
        }
    }

    private void ChangeState(E_BattleState state)
    {
        _battleState = state;

        switch (_battleState)
        {
            case E_BattleState.PlayerTurn:
                SetAttackView();

                break;
            case E_BattleState.EnemyTurn:
                EnemyTurn();

                break;
            case E_BattleState.Waiting:
                break;
        }
    }

    private void EnemyTurn()
    {
        BattleQueueElement playerElement = _playerGroup[UnityEngine.Random.Range(0, _playerGroup.Count)];

        playerElement.Troop.Attack(BattleQueue[_currentQueueElement].Troop.CurrentDamage);

        onActionPerformed?.Invoke($"Team {BattleQueue[_currentQueueElement].Team} attack {playerElement.Troop.UnitStats.id} with {BattleQueue[_currentQueueElement].Troop.CurrentDamage} damage. HP left: {playerElement.Troop.CurrentHealth}");

        EndTurn();

        SelectNextElement();
    }

    private void OnTroopDied(BattleQueueElement troop)
    {
        Debug.Log($"Troop died: {troop.Troop.UnitStats.id}");

        BattleQueue.Remove(troop);

        if (troop.Team == BattleQueueElement.E_ElementTeam.Player)
            _playerGroup.Remove(troop);
        else
            _enemyGroup.Remove(troop);

        Destroy(troop.TroopObject.gameObject);

        if (_playerGroup.Count == 0)
        {
            WindowManager.Instance.GetWindow<MessageWindow>().Show("Lose");
        }
        else if (_enemyGroup.Count == 0)
            WindowManager.Instance.GetWindow<MessageWindow>().Show("Win");

        ChangeState(E_BattleState.Waiting);  
    }

    public void PerformTurn()
    {
        if (_selectedSkill >= 0)
        {
            List<Troop> skillTargets = new List<Troop>();

            for (int loop = 0; loop < _selectedTroops.Count; loop++)
                skillTargets.Add(_selectedTroops[loop].Troop);

            BattleQueue[_currentQueueElement].UseSkill(BattleQueue[_currentQueueElement].Troop.UnitStats.skills[_selectedSkill], skillTargets, 10);
        }
        else
        {
            _selectedTroops[0].Troop.Attack(BattleQueue[_currentQueueElement].Troop.CurrentDamage);

            onActionPerformed?.Invoke($"Team {BattleQueue[_currentQueueElement].Team} attack {_selectedTroops[0].Troop.UnitStats.id} with {BattleQueue[_currentQueueElement].Troop.CurrentDamage} damage. HP left: {_selectedTroops[0].Troop.CurrentHealth}");
        }

        EndTurn();

        SelectNextElement();   
    }

    private void EndTurn()
    {
        BattleQueueElement queueElement = BattleQueue[_currentQueueElement];

        BattleQueue.Remove(queueElement);
        BattleQueue.Add(queueElement);

        _selectedTroops.Clear();
        _selectedSkill = -1;
    }

    public void SetAttackView()
    {
        Debug.Log("SetAttack");

        for (int loop = 0; loop < Mathf.Max(_playerGroup.Count, _enemyGroup.Count); loop++)
        {
            if (loop < _enemyGroup.Count)
            {
                _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(true);
                _enemyGroup[loop].TroopObject.HighlightImage.color = Color.red;
            }

            if (loop < _playerGroup.Count)
                _playerGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(false);
        }
    }

    public void SetAttackTroop(BattleQueueElement queueElement)
    {
        for (int loop = 0; loop < _enemyGroup.Count; loop++)
        {
            _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(queueElement == _enemyGroup[loop]);
        }
    }

    public void SetAttackView(Skill activeSkill = null)
    {
        if (activeSkill != null)
            _selectedSkill = activeSkill.Id;

        switch (activeSkill.SkillTarget)
        {
            case Skill.E_SkillUsageTarget.All:
                for (int loop = 0; loop < Mathf.Max(_playerGroup.Count, _enemyGroup.Count); loop++)
                {
                    if (loop < _enemyGroup.Count)
                    {
                        _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(true);
                        _enemyGroup[loop].TroopObject.HighlightImage.color = Color.red;
                    }

                    if (loop < _playerGroup.Count)
                    {
                        _playerGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(true);
                        _playerGroup[loop].TroopObject.HighlightImage.color = Color.red;
                    }
                }

                break;
            case Skill.E_SkillUsageTarget.Enemy:
                for (int loop = 0; loop < Mathf.Max(_playerGroup.Count, _enemyGroup.Count); loop++)
                {
                    if (loop < _enemyGroup.Count)
                    {
                        _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(true);
                        _enemyGroup[loop].TroopObject.HighlightImage.color = Color.red;
                    }

                    if (loop < _playerGroup.Count)
                        _playerGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(false);
                }

                break;
            case Skill.E_SkillUsageTarget.Player:
                for (int loop = 0; loop < Mathf.Max(_playerGroup.Count, _enemyGroup.Count); loop++)
                {
                    if (loop < _enemyGroup.Count)
                        _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(false);

                    if (loop < _playerGroup.Count)
                    {
                        _playerGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(true);
                        _playerGroup[loop].TroopObject.HighlightImage.color = Color.green;
                    }
                }

                break;
        }
    }

    private void OnSkillUsed()
    {

    }

    private void OnClickTroop(BattleQueueElement selectedTroop)
    {
        if (_battleState != E_BattleState.PlayerTurn)
            return;

        if (_selectedSkill == -1)
        {
            _selectedTroops.Add(selectedTroop);

            SetAttackTroop(selectedTroop);
        }
    }
}
