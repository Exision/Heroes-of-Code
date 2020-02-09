using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterfaceController : MonoBehaviour
{
    public Action onClickPerformTurnButton;
    public Action<int> onSkillSelect;

    [SerializeField] private BattleQueueView _battleQueueViewPrefab;
    [SerializeField] private BattleSkillView _battleSkillViewPrefab;
    [SerializeField] private Text _battleLog;
    [SerializeField] private TextFly _damageTextFly;

    private BattleController _battleController;

    private List<BattleQueueView> _battleQueueViews = new List<BattleQueueView>();
    private List<BattleSkillView> _battleSkillViews = new List<BattleSkillView>();

    private int _selectedSkill = -1;

    private void SubscribeToEvents()
    {
        _battleController.onActionPerformed += OnActionPerformed;
        _battleController.onCurrentElementChanged += OnElementChanged;
    }

    private void OnDisable()
    {
        _battleController.onCurrentElementChanged -= OnElementChanged;
        _battleController.onActionPerformed -= OnActionPerformed;
    }

    private void Start()
    {
        _battleSkillViewPrefab.gameObject.SetActive(false);
    }

    public void Init(BattleController controller)
    {
        _battleController = controller;

        SubscribeToEvents();
        CreateBattleQueueView();
    }

    private void CreateBattleQueueView()
    {
        for (int loop = 0; loop < _battleController.BattleQueue.Count; loop++)
        {
            BattleQueueView queueView = Instantiate<BattleQueueView>(_battleQueueViewPrefab, _battleQueueViewPrefab.transform.parent);
            queueView.Init(_battleController.BattleQueue[loop]);

            _battleQueueViews.Add(queueView);
        }

        Destroy(_battleQueueViewPrefab.gameObject);
    }

    #region Skills view update
    private void UpdateQueueViews()
    {
        for (int loop = 0; loop < _battleQueueViews.Count; loop++)
        {
            if (loop >= _battleController.BattleQueue.Count)
                _battleQueueViews[loop].gameObject.SetActive(false);
            else
            {
                _battleQueueViews[loop].gameObject.SetActive(true);
                _battleQueueViews[loop].Init(_battleController.BattleQueue[loop]);
            }
        }
    }

    private void OnElementChanged()
    {
        _battleQueueViews[0].SetSelected(false);

        if (_selectedSkill > -1)
            _battleSkillViews[_selectedSkill].SetSelected(false);

        _selectedSkill = -1;

        UpdateQueueViews();

        _battleQueueViews[0].SetSelected(true);

        SetSkillView(_battleController.BattleQueue[0]);
    }

    public void SetSkillView(BattleQueueElement currentTroop)
    {
        for (int loop = 0; loop < _battleSkillViews.Count; loop++)
            _battleSkillViews[loop].gameObject.SetActive(false);

        if (currentTroop.Troop.UnitStats.skills.Length > 0 && currentTroop.UsedSkills.Count != currentTroop.Troop.UnitStats.skills.Length)
        {
            for (int loop = 0; loop < currentTroop.Troop.UnitStats.skills.Length; loop++)
            {
                if (currentTroop.UsedSkills.Contains(currentTroop.Troop.UnitStats.skills[loop]))
                    continue;

                if (loop >= _battleSkillViews.Count)
                {
                    BattleSkillView skillView = Instantiate<BattleSkillView>(_battleSkillViewPrefab, _battleSkillViewPrefab.transform.parent);

                    _battleSkillViews.Add(skillView);
                }

                _battleSkillViews[loop].Init(loop, currentTroop.Troop.UnitStats.skills[loop]);
                _battleSkillViews[loop].onSkillSelect = OnSkillSelect;
                _battleSkillViews[loop].gameObject.SetActive(true);
            }
        }
    }
    #endregion

    private void OnSkillSelect(int skillIndex)
    {
        if (_selectedSkill > -1)
        {
            _battleSkillViews[_selectedSkill].SetSelected(false);

            _battleSkillViews[skillIndex].SetSelected(_selectedSkill != skillIndex);
        }
        else
            _battleSkillViews[skillIndex].SetSelected(true);

        _selectedSkill = _selectedSkill != skillIndex ? skillIndex : -1;

        onSkillSelect?.Invoke(_selectedSkill);
    }

    private void OnActionPerformed(string log)
    {
        _battleLog.text = $"{_battleLog.text}\n{log}\n";
    }

    public void OnAttackButtonClick()
    {
        onClickPerformTurnButton?.Invoke();
    }

    public void ShowDamageFly(string text, Vector3 fromPosition, Color color)
    {
        TextFly textFly = Instantiate<TextFly>(_damageTextFly).GetComponent<TextFly>();
        textFly.Play(text, fromPosition, color);
    }
}
