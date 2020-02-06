using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Action onCurrentElementChanged;
    public Action<BattleQueueElement> onTroopAttack;
    public Action<string> onActionPerformed;

    [SerializeField] private BattleInterfaceController _battleInterface;

    public List<BattleQueueElement> BattleQueue { get; private set; } = new List<BattleQueueElement>();

    private E_BattleState _battleState = E_BattleState.Waiting;

    private List<BattleQueueElement> _playerGroup = new List<BattleQueueElement>();
    private List<BattleQueueElement> _enemyGroup = new List<BattleQueueElement>();

    private int _selectedSkill = -1;
    private List<BattleQueueElement> _selectedTroops = new List<BattleQueueElement>();

    private WaitForSeconds _animationWait = new WaitForSeconds(0.3f);

    private void OnEnable()
    {
        _battleInterface.onClickPerformTurnButton += PerformTurn;
        _battleInterface.onSkillSelect += OnSkillSelect;
    }

    private void OnDisable()
    {
        _battleInterface.onClickPerformTurnButton -= PerformTurn;
        _battleInterface.onSkillSelect -= OnSkillSelect;
    }

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
                    hits[loop].collider.gameObject.GetComponent<TroopObject>()?.OnClickTroop();
            }
        }
    }

    #region Initialization
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
            BattleQueueElement queueElement = new BattleQueueElement(enemyTroops[loop], enemyObjects[loop], E_BatteElementTeam.Enemy);
            queueElement.onDamageReceived = OnDamageReceived;
            queueElement.onHealReceived = OnHealReceived;
            queueElement.onClickTroop = OnClickTroop;
            queueElement.onPerformAttack = OnPerformAttack;
            queueElement.onSkillUsed = OnSkillUsed;
            queueElement.onTroopDied = OnTroopDied;

            _enemyGroup.Add(queueElement);
        }
    }

    private void InitPlayer()
    {
        TroopObject[] playerObjects = new PlayerFactory().Create(GameController.Instance.PlayerData.Group);

        for (int loop = 0; loop < playerObjects.Length; loop++)
        {
            BattleQueueElement queueElement = new BattleQueueElement(GameController.Instance.PlayerData.Group[loop], playerObjects[loop], E_BatteElementTeam.Player);
            queueElement.onDamageReceived = OnDamageReceived;
            queueElement.onHealReceived = OnHealReceived;
            queueElement.onClickTroop = OnClickTroop;
            queueElement.onPerformAttack = OnPerformAttack;
            queueElement.onSkillUsed = OnSkillUsed;
            queueElement.onTroopDied = OnTroopDied;

            _playerGroup.Add(queueElement);
        }
    }

    // Создание очереди боя
    private void CreateQueue()
    {
        for (int loop = 0; loop < GameConfig.FindMultiple(_playerGroup.Count, _enemyGroup.Count); loop++)
        {
            BattleQueue.Add(loop < _playerGroup.Count ? _playerGroup[loop] : _playerGroup[loop % _playerGroup.Count]);

            BattleQueue.Add(loop < _enemyGroup.Count ? _enemyGroup[loop] : _enemyGroup[loop % _enemyGroup.Count]);
        }
    }
    #endregion

    private void SelectNextElement()
    {
        onCurrentElementChanged?.Invoke();

        DeselectAll();

        BattleQueue[0].TroopObject.ActiveImage.gameObject.SetActive(true);

        switch (BattleQueue[0].Team)
        {
            case E_BatteElementTeam.Player:
                ChangeState(E_BattleState.PlayerTurn);
                break;
            case E_BatteElementTeam.Enemy:
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
                break;
            case E_BattleState.EnemyTurn:
                EnemyTurn();
                break;
            case E_BattleState.Waiting:
                break;
        }
    }
    
    // Случайный ход для противника, в идеале должен быть свой контроллер для противников учитывающий тип юнита, информацию о битве и т.д., но в данном случае им пренебрегли
    private void EnemyTurn()
    {
        BattleQueueElement currentTroop = BattleQueue[0];

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

                    break;
                case Skill.E_SkillUsageTarget.Enemy:
                    skillTargets.Add(_playerGroup[UnityEngine.Random.Range(0, _playerGroup.Count)]);

                    break;
                case Skill.E_SkillUsageTarget.Ally:
                    skillTargets.Add(_enemyGroup[UnityEngine.Random.Range(0, _enemyGroup.Count)]);

                    break;
            }

            StartCoroutine(SkillAnimation(skill, BattleQueue[0], skillTargets));
        }
        else
            StartCoroutine(AttackAnimation(BattleQueue[0], _playerGroup[UnityEngine.Random.Range(0, _playerGroup.Count)]));
    }

    public void PerformTurn()
    {
        if (_selectedTroops.Count > 0)
        {
            if (_selectedSkill >= 0)
            {
                if (BattleQueue[0].Troop.UnitStats.skills[_selectedSkill].SkillTarget == Skill.E_SkillUsageTarget.All)
                {
                    _selectedTroops.Clear();

                    _selectedTroops.AddRange(_playerGroup);
                    _selectedTroops.AddRange(_enemyGroup);
                }

                StartCoroutine(SkillAnimation(BattleQueue[0].Troop.UnitStats.skills[_selectedSkill], BattleQueue[0], _selectedTroops));
            }
            else
                StartCoroutine(AttackAnimation(BattleQueue[0], _selectedTroops[0]));
        }
        else
        {
            MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
            messageWindow.onClickOkButton = () =>
            {
                messageWindow.Hide();
            };
            messageWindow.Show(Localization.Instance.Get("battle_select_enemy"));
        }
    }

    // Процесс атаки юнита
    private IEnumerator AttackAnimation(BattleQueueElement performer, BattleQueueElement target)
    {
        Vector3 performerStartPosition = performer.TroopObject.transform.position;

        performer.Move(target.TroopObject.transform.position + (target.Team == E_BatteElementTeam.Enemy ? Vector3.left : Vector3.right));

        yield return _animationWait;

        performer.Attack(target);

        performer.Move(performerStartPosition);

        yield return _animationWait;

        EndTurn();

        SelectNextElement();

        yield break;
    }

    // Использование способности (простейшая анимация и сама способность)
    private IEnumerator SkillAnimation(Skill skill, BattleQueueElement performer, List<BattleQueueElement> targets)
    {
        Vector3 performerStartPosition = performer.TroopObject.transform.position;

        performer.Move(performer.Team == E_BatteElementTeam.Player ? Vector3.right : Vector3.left);

        yield return _animationWait;

        performer.UseSkill(skill, targets);

        performer.Move(performerStartPosition);

        yield return _animationWait;

        EndTurn();

        SelectNextElement();

        yield break;
    }

    // Перемещение юнита в конец списка, удаление двойных записей (один и тот же юнит несколько раз подряд)
    private void EndTurn()
    {
        BattleQueueElement queueElement = BattleQueue[0];

        queueElement.TroopObject.ActiveImage.gameObject.SetActive(false);

        if (_playerGroup.Count <= 0 || _enemyGroup.Count <= 0)
            EndBattle();

        BattleQueue.Remove(queueElement);
        BattleQueue.Add(queueElement);

        _selectedTroops.Clear();
        _selectedSkill = -1;

        for (int loop = 0; loop < BattleQueue.Count - 1; loop++)
        {
            if (loop >= BattleQueue.Count)
                break;

            if (BattleQueue[loop] == BattleQueue[loop + 1])
                BattleQueue.RemoveAt(loop + 1);
        }
    }

    // Завершение боя, определение победителя
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


    #region Objects view
    public void SetTroopSelected(BattleQueueElement target)
    {
        switch (target.Team)
        {
            case E_BatteElementTeam.Player:
                for (int loop = 0; loop < _playerGroup.Count; loop++)
                    _playerGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(_selectedTroops.Contains(_playerGroup[loop]));

                break;
            case E_BatteElementTeam.Enemy:
                for (int loop = 0; loop < _enemyGroup.Count; loop++)
                    _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(_selectedTroops.Contains(_enemyGroup[loop]));

                break;
        }
    }

    public void SetAttackTroop(List<BattleQueueElement> targets)
    {
        for (int loop = 0; loop < _enemyGroup.Count; loop++)
            _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(targets.Contains(_enemyGroup[loop]));
    }

    private void DeselectAll()
    {
        for (int loop = 0; loop < Mathf.Max(_playerGroup.Count, _enemyGroup.Count); loop++)
        {
            if (loop < _enemyGroup.Count)
                _enemyGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(false);

            if (loop < _playerGroup.Count)
                _playerGroup[loop].TroopObject.HighlightImage.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Troops events
    private void OnTroopDied(BattleQueueElement troop)
    {
        BattleQueue.RemoveAll(item => item == troop);

        Destroy(troop.TroopObject.gameObject);

        if (troop.Team == E_BatteElementTeam.Player)
            _playerGroup.Remove(troop);
        else
            _enemyGroup.Remove(troop);
    }

    private void OnHealReceived(BattleQueueElement target, int healAmount)
    {
        _battleInterface.ShowDamageFly(healAmount.ToString(), target.TroopObject.transform.position, Color.green);
    }

    private void OnDamageReceived(BattleQueueElement target, int damage)
    {
        _battleInterface.ShowDamageFly(damage.ToString(), target.TroopObject.transform.position, Color.red);
    }

    private void OnPerformAttack(BattleQueueElement target, int damage)
    {
        onActionPerformed?.Invoke(string.Format(Localization.Instance.Get("action_log_attack"),
            Localization.Instance.Get("unit_name_" + BattleQueue[0].Troop.UnitStats.id),
            BattleQueue[0].Team.ToString(),
            Localization.Instance.Get("unit_name_" + target.Troop.UnitStats.id),
            target.Team.ToString(),
            damage,
            target.Troop.CurrentHealth));
    }

    private void OnSkillUsed(Skill skill)
    {
        onActionPerformed?.Invoke(string.Format(Localization.Instance.Get("action_log_skill"),
            Localization.Instance.Get("unit_name_" + BattleQueue[0].Troop.UnitStats.id),
            BattleQueue[0].Team.ToString(),
            Localization.Instance.Get("skill_name_" + skill.Id)));
    }

    private void OnClickTroop(BattleQueueElement selectedTroop)
    {
        if (_battleState != E_BattleState.PlayerTurn
            || (selectedTroop.Team == E_BatteElementTeam.Player && (_selectedSkill <= -1 || BattleQueue[0].Troop.UnitStats.skills[_selectedSkill].SkillTarget != Skill.E_SkillUsageTarget.Ally))
            || (selectedTroop.Team != E_BatteElementTeam.Player && (_selectedSkill > -1 && BattleQueue[0].Troop.UnitStats.skills[_selectedSkill].SkillTarget == Skill.E_SkillUsageTarget.Ally)))
            return;

        if (_selectedSkill <= -1
            || (_selectedSkill > -1 && !BattleQueue[0].Troop.UnitStats.skills[_selectedSkill].IsMultiTarget))
            _selectedTroops.Clear();

        if (!_selectedTroops.Contains(selectedTroop))
            _selectedTroops.Add(selectedTroop);
        else
            _selectedTroops.Remove(selectedTroop);

        SetTroopSelected(selectedTroop);
    }

    private void OnSkillSelect(int skillId)
    {
        _selectedSkill = skillId;

        _selectedTroops.Clear();

        DeselectAll();
    }
    #endregion
}
