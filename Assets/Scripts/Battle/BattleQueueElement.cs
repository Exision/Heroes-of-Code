using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQueueElement
{
    public Action onSkillUsed;
    public Action<BattleQueueElement> onClickTroop;
    public Action<BattleQueueElement> onTroopDied;

    public Troop Troop { get; private set; }

    public TroopObject TroopObject { get; private set; }

    public E_ElementTeam Team { get; private set; }

    public List<Skill> UsedSkills { get; private set; } = new List<Skill>();

    public BattleQueueElement(Troop troop, TroopObject troopObject, E_ElementTeam team)
    {
        Troop = troop;
        TroopObject = troopObject;
        Team = team;

        TroopObject.onClickTroop = OnClickTroop;
        Troop.onTroopDied = OnTroopDied;
    }

    public void UseSkill(Skill skill, List<Troop> targets, float power)
    {
        bool isExist = false;

        for (int loop = 0; loop < Troop.UnitStats.skills.Length; loop++)
        {
            if (Troop.UnitStats.skills[loop] == skill)
            {
                isExist = true;

                break;
            }
        }
        if (!isExist || UsedSkills.Contains(skill))
            return;

        UsedSkills.Add(skill);

        skill.Use(targets, power);

        onSkillUsed?.Invoke();
    }

    private void OnClickTroop()
    {
        onClickTroop?.Invoke(this);
    }

    private void OnTroopDied()
    {
        Debug.Log("BattleQueueElement OnTroopDied");

        onTroopDied?.Invoke(this);
    }

    public override bool Equals(object other)
    {
        if (other == null || !this.GetType().Equals(other.GetType()))
            return false;

        BattleQueueElement otherElement = other as BattleQueueElement;

        return Troop.UnitStats.id == otherElement.Troop.UnitStats.id && Team == otherElement.Team;
    }

    public enum E_ElementTeam
    {
        Player,
        Enemy
    }

    public override int GetHashCode()
    {
        int hash = 13;
        hash = hash * 7 + Troop.UnitStats.id.GetHashCode();
        hash = hash * 7 + Team.GetHashCode();

        return hash;
    }
}
