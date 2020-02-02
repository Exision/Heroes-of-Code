using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillView : MonoBehaviour
{
    public Action<int, Skill> onSkillSelect;

    [SerializeField] private Image _skillIcon;
    [SerializeField] private Text _skillName;
    [SerializeField] private Text _skillDescription;
    [SerializeField] private Image _skillSelectedImage;

    private int _index;
    private Skill _skill;

    public void Init(int index, Skill skill)
    {
        _index = index;
        _skill = skill;

        _skillName.text = Localization.Instance.Get("skill_name_" + _skill.Id);
        _skillDescription.text = Localization.Instance.Get("skill_desc_" + _skill.Id);
        _skillIcon.sprite = Resources.Load<Sprite>($"{GameConfig.Instance.skillImagePath}SkillIcon_{_skill.Id}");

        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        _skillSelectedImage.gameObject.SetActive(isSelected);
    }

    public void OnSkillSelect()
    {
        onSkillSelect?.Invoke(_index, _skill);
    }
}
