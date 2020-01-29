using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Action<UnitStats> onUnitAttacked;
    public Action<UnitStats> onUnitDead;

    public List<Troop> Group { get; private set; }

    public void Init(Dictionary<int, int> group)
    {
        foreach (KeyValuePair<int,int> item in group)
        {
            Troop unit = new Troop(GameController.Instance.UnitsManager.Units[item.Key], item.Value);

            if (!Group.Contains(unit))
                Group.Add(unit);
        }
    }
}
