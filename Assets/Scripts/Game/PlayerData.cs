using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<Troop> Group { get; private set; } = new List<Troop>();
    public Vector3 Position { get; set; }

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

        Position = GameConfig.Instance.playerStartPosition;
    }

    public PlayerData(List<Troop> group)
    {
        Group = group;

        Position = GameConfig.Instance.playerStartPosition;
    }

    public void SetGroup(List<Troop> group)
    {
        Group = group;
    }

    public void SetGroup(Dictionary<int, int> group)
    {
        Group.Clear();

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
