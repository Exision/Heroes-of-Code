using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGroupWindow : Window
{
    public Action<Dictionary<int, int>> onGroupReady;

    [SerializeField] private UnitCell _unitCellPrefab;

    private List<UnitCell> _unitCells;
    private Dictionary<int, int> _group;

    public override void Show(bool animation = true)
    {
        base.Show(animation);

        InitUnits();
    }

    private void InitUnits()
    {
        Debug.Log(GameController.Instance.UnitsManager.Units.Length);

        foreach (UnitStats unit in GameController.Instance.UnitsManager.Units)
        {
            UnitCell unitCell = Instantiate<UnitCell>(_unitCellPrefab, _unitCellPrefab.transform.parent);
            unitCell.Init(unit);
            unitCell.onValueChanged = OnUnitCountChanged;
        }

        Destroy(_unitCellPrefab);
    }

    private void OnUnitCountChanged(int unitId, int unitCount)
    {
        if (_group.ContainsKey(unitId))
            _group[unitId] = unitCount;
        else
            _group.Add(unitId, unitCount);
    }

    public void OnClickReadyButton()
    {
        onGroupReady?.Invoke(_group);
    }
}
