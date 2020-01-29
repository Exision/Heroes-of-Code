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
    [SerializeField] private Text _inputFieldText;

    private UnitStats _unit;

    public void Init(UnitStats unit)
    {
        _unit = unit;

        _unitNameText.text = Localization.Instance.Get(_unit.unitName);
        _unitIcon.sprite = Resources.Load<Sprite>($"Sprites/{_unit.unitName}Icon");
    }

    public void OnValueChanged(string text)
    {
        if (int.TryParse(text, out int count))
            onValueChanged?.Invoke(_unit.id, count);
    }
}
