using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<Troop> Group { get; private set; } = new List<Troop>();

    public PlayerData(Dictionary<int, int> group)
    {
        if (group.Count > 0)
        {
            foreach (KeyValuePair<int, int> item in group)
            {
                Troop unit = new Troop(GameController.Instance.UnitsStorage.Units[item.Key], item.Value);

                if (!Group.Contains(unit))
                    Group.Add(unit);
            }
        }
    }
}
