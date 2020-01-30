using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGroupWindow : Window
{
    public Action<Dictionary<int, int>> onGroupReady;

    [SerializeField] private UnitCell _unitCellPrefab;

    private List<UnitCell> _unitCells;
    private Dictionary<int, int> _group = new Dictionary<int, int>();

    public override void Show(bool animation = true)
    {
        base.Show(animation);

        InitUnits();
    }

    private void InitUnits()
    {
        foreach (UnitStats unit in GameController.Instance.UnitsStorage.Units)
        {
            UnitCell unitCell = Instantiate<UnitCell>(_unitCellPrefab, _unitCellPrefab.transform.parent);
            unitCell.Init(unit);
            unitCell.onValueChanged = OnUnitCountChanged;
        }

        Destroy(_unitCellPrefab.gameObject);
    }

    private void OnUnitCountChanged(int unitId, int unitCount)
    {
        if (_group.ContainsKey(unitId))
        {
            if (unitCount > 0)
                _group[unitId] = unitCount;
            else
                _group.Remove(unitId);
        }
        else if (unitCount > 0)
            _group.Add(unitId, unitCount);

    }

    public void OnClickReadyButton()
    {
        if (_group.Count == 0)
        {
            MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
            messageWindow.onClickOkButton = () => messageWindow.Hide();
            messageWindow.Show("error_empty_group");
        }
        else
            onGroupReady?.Invoke(_group);
    }
}
