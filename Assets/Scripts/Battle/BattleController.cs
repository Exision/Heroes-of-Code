using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Action<int> onCurrentElementChanged;

    [SerializeField] private BattleInterfaceController _battleInterface;

    public List<BattleQueueElement> BattleQueue { get; private set; } = new List<BattleQueueElement>();

    private E_BattleState _battleState = E_BattleState.Waiting;

    private List<BattleQueueElement> _playerGroup = new List<BattleQueueElement>();
    private List<BattleQueueElement> _enemyGroup = new List<BattleQueueElement>();

    private int _currentQueueElement = 0;
    private int _selectedSkill = -1;
    private BattleQueueElement _selectedTroops;

    private WaitForSeconds _animationWait = new WaitForSeconds(0.3f);

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
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.allCameras[1].ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);

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
        List<Troop> enemyTroops = GameController.Instance.EnemysDatas[GameController.Instance.CurrentEnemy].Group;

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
    }

    private void SelectNextElement()
    {
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
        StartCoroutine(EnemyTurnCoroutine());

        /*
        BattleQueueElement currentTroop = BattleQueue[_currentQueueElement];

        bool isHaveSkills = currentTroop.Troop.UnitStats.skills.Length > 0 && currentTroop.UsedSkills.Count != currentTroop.Troop.UnitStats.skills.Length;

        if (isHaveSkills && UnityEngine.Random.value < GameConfig.Instance.enemyUseSkillChance)
        {
            Skill skill = currentTroop.Troop.UnitStats.skills[UnityEngine.Random.Range(0, currentTroop.Troop.UnitStats.skills.Length)];

            List<BattleQueueElement> skillTargets = new List<BattleQueueElement>();

            switch (skill.SkillTarget)
            {
                case Skill.E_SkillUsageTarget.All:
                    skillTargets.AddRange(_playerGroup);
                    skillTargets.AddRange(_enemyGroup);

                    currentTroop.UseSkill(skill, skillTargets, currentTroop.Troop.CurrentDamage));

                    break;
                    /*
                case Skill.E_SkillUsageTarget.Enemy:
                case Skill.E_SkillUsageTarget.Player:
            }
        }

        BattleQueueElement playerElement = _playerGroup[UnityEngine.Random.Range(0, _playerGroup.Count)];

        playerElement.Troop.Attack(BattleQueue[_currentQueueElement].Troop.CurrentDamage);

        onActionPerformed?.Invoke($"Team {BattleQueue[_currentQueueElement].Team} attack {playerElement.Troop.UnitStats.id} with {BattleQueue[_currentQueueElement].Troop.CurrentDamage} damage. HP left: {playerElement.Troop.CurrentHealth}");

        EndTurn();

        SelectNextElement();
        */
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        BattleQueueElement playerElement = _playerGroup[UnityEngine.Random.Range(0, _playerGroup.Count)];

        yield return AttackAnimation(BattleQueue[_currentQueueElement], playerElement);

        EndTurn();

        SelectNextElement();

        yield break;
    }

    public void PerformTurn()
    {
        StartCoroutine(PerformTurnCoroutine());
    }
    
    private IEnumerator PerformTurnCoroutine()
    {
        if (_selectedTroops != null)
        {
            if (_selectedSkill >= 0)
            {

            }
            else
                yield return AttackAnimation(BattleQueue[_currentQueueElement], _selectedTroops);
        }

        EndTurn();

        SelectNextElement();

        yield break;
    }

    private IEnumerator AttackAnimation(BattleQueueElement performer, BattleQueueElement target)
    {
        Vector3 performerStartPosition = performer.TroopObject.transform.position;

        performer.TroopObject.transform.position =
            Vector3.MoveTowards(performer.TroopObject.transform.position,
            target.TroopObject.transform.position + (target.Team == BattleQueueElement.E_ElementTeam.Enemy ? Vector3.left : Vector3.right),
            GameConfig.Instance.unitSpeed);

        yield return _animationWait;

        target.Troop.Attack(performer.Troop.CurrentDamage);

        performer.TroopObject.transform.position = Vector3.MoveTowards(performer.TroopObject.transform.position, performerStartPosition, GameConfig.Instance.unitSpeed);

        yield return _animationWait;

        yield break;
    }

    private void EndTurn()
    {
        BattleQueueElement queueElement = BattleQueue[_currentQueueElement];

        if (_playerGroup.Count == 0 || _enemyGroup.Count == 0)
            EndBattle();

        BattleQueue.Remove(queueElement);
        BattleQueue.Add(queueElement);

        _selectedTroops = null;
        _selectedSkill = -1;

        for (int loop = 0; loop < BattleQueue.Count - 1; loop++)
        {
            if (loop >= BattleQueue.Count)
                break;

            if (BattleQueue[loop] == BattleQueue[loop + 1])
                BattleQueue.RemoveAt(loop + 1);
        }
    }

    private void OnTroopDied(BattleQueueElement troop)
    {
        BattleQueue.RemoveAll(item => item == troop);

        Destroy(troop.TroopObject.gameObject);

        if (troop.Team == BattleQueueElement.E_ElementTeam.Player)
            _playerGroup.Remove(troop);
        else
            _enemyGroup.Remove(troop);
    }

    private void EndBattle()
    {
        ChangeState(E_BattleState.Waiting);

        bool isWin = _enemyGroup.Count == 0;

        if (isWin)
        {
            MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
            messageWindow.onClickOkButton = () =>
            {
                messageWindow.Hide();

                List<Troop> group = new List<Troop>();

                for (int loop = 0; loop < _playerGroup.Count; loop++)
                    group.Add(_playerGroup[loop].Troop);

                GameController.Instance.EndBattle(group);
            };
            messageWindow.Show(Localization.Instance.Get("win"));
        }
        else
        {
            MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
            messageWindow.onClickOkButton = () =>
            {
                messageWindow.Hide();

                GameController.Instance.EndGame();
            };
            messageWindow.Show(Localization.Instance.Get("lose"));
        }
    }

    public void SetAttackView()
    {
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
            _selectedTroops = selectedTroop;

            SetAttackTroop(selectedTroop);
        }
    }
}
