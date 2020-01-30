using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCell : MonoBehaviour
{
    public Action<int, int> onValueChanged;

    [SerializeField] private Text _unitNameText;
    [SerializeField] private Image _unitIcon;
    [SerializeField] private InputField _inputField;

    private UnitStats _unit;

    public void Init(UnitStats unit)
    {
        _unit = unit;

        _unitNameText.text = Localization.Instance.Get("unit_name_" + _unit.id);
        _unitIcon.sprite = Resources.Load<Sprite>($"{GameConfig.Instance.resourcesSpritesPath}UnitIcon_{_unit.id}");
    }

    public void OnValueChanged()
    {
        if (int.TryParse(_inputField.text, out int count))
            onValueChanged?.Invoke(_unit.id, count);
    }
}
