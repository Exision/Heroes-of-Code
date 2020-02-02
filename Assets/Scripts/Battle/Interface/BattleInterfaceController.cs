using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInterfaceController : MonoBehaviour
{
    [SerializeField] private Text _battleLog;
    [SerializeField] private BattleQueueView _battleQueueViewPrefab;
    [SerializeField] private BattleSkillView _battleSkillViewPrefab;

    private BattleController _battleController;

    private List<BattleQueueView> _battleQueueViews = new List<BattleQueueView>();

    private List<BattleSkillView> _battleSkillViews = new List<BattleSkillView>();

    private int _currentTroop = -1;

    private int _selectedSkill = -1;

    private void SubscribeToEvents()
    {
        _battleController.onActionPerformed += OnActionPerformed;
        _battleController.onCurrentElementChanged += OnElementChanged;
    }

    private void OnDisable()
    {
        _battleController.onActionPerformed -= OnActionPerformed;
        _battleController.onCurrentElementChanged -= OnElementChanged;
    }

    private void Start()
    {
        _battleLog.text = "";

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

    private void UpdateQueueViews()
    {
        for (int loop = 0; loop < _battleController.BattleQueue.Count; loop++)
        {
            _battleQueueViews[loop].Init(_battleController.BattleQueue[loop]);
        }
    }

    private void OnElementChanged(int currentElement)
    {
        if (_currentTroop > -1)
            _battleQueueViews[_currentTroop].SetSelected(false);

        if (_selectedSkill > -1)
            _battleSkillViews[_selectedSkill].SetSelected(false);

        _selectedSkill = -1;
        _currentTroop = currentElement;

        UpdateQueueViews();

        _battleQueueViews[_currentTroop].SetSelected(true);

        SetSkillView(_battleController.BattleQueue[_currentTroop].Troop);
    }

    public void SetSkillView(Troop currentTroop)
    {
        for (int loop = 0; loop < _battleSkillViews.Count; loop++)
            _battleSkillViews[loop].gameObject.SetActive(false);

        if (currentTroop.UnitStats.skills.Length > 0)
        {
            for (int loop = 0; loop < currentTroop.UnitStats.skills.Length; loop++)
            {
                if (loop >= _battleSkillViews.Count)
                {
                    BattleSkillView skillView = Instantiate<BattleSkillView>(_battleSkillViewPrefab, _battleSkillViewPrefab.transform.parent);

                    _battleSkillViews.Add(skillView);
                }

                _battleSkillViews[loop].Init(loop, currentTroop.UnitStats.skills[loop]);
                _battleSkillViews[loop].onSkillSelect = OnSkillSelect;
                _battleSkillViews[loop].gameObject.SetActive(true);
            }
        }
    }

    private void OnSkillSelect(int skillIndex, Skill selectedSkill)
    {
        if (_selectedSkill == skillIndex)
        {
            _battleSkillViews[_selectedSkill].SetSelected(false);

            _battleController.SetAttackView();

            _selectedSkill = -1;

            return;
        }

        _selectedSkill = skillIndex;

        _battleSkillViews[_selectedSkill].SetSelected(true);

        _battleController.SetAttackView(selectedSkill);
    }

    public void OnAttackButtonClick()
    {
        _battleController.PerformTurn();
    }

    private void OnActionPerformed(string log)
    {
        _battleLog.text = _battleLog.text + "\n" + log;
    }
}
