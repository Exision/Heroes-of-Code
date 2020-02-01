using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQueueElement
{
    public Troop Troop { get; private set; }

    public TroopObject TroopObject { get; private set; }

    public E_ElementTeam Team { get; private set; }

    public BattleQueueElement(Troop troop, TroopObject troopObject, E_ElementTeam team)
    {
        Troop = troop;
        TroopObject = troopObject;
        Team = team;
    }

    public enum E_ElementTeam
    {
        Player,
        Enemy
    }
}
