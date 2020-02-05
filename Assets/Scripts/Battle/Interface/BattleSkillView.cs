using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSkillView : MonoBehaviour
{
    public Action<int> onSkillSelect;

    [SerializeField] private Image _skillIcon;
    [SerializeField] private Text _skillName;
    [SerializeField] private Text _skillDescription;
    [SerializeField] private Image _skillSelectedImage;

    private int _index;

    public void Init(int index, Skill skill)
    {
        _index = index;

        _skillName.text = Localization.Instance.Get("skill_name_" + skill.Id);
        _skillDescription.text = Localization.Instance.Get("skill_desc_" + skill.Id);
        _skillIcon.sprite = Resources.Load<Sprite>($"{GameConfig.Instance.skillImagePath}SkillIcon_{skill.Id}");

        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        _skillSelectedImage.gameObject.SetActive(isSelected);
    }

    public void OnSkillSelect()
    {
        onSkillSelect?.Invoke(_index);
    }
}
