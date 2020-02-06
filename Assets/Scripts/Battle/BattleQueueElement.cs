using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleQueueElement
{
    public Action<BattleQueueElement, int> onPerformAttack;
    public Action<Skill> onSkillUsed;
    public Action<BattleQueueElement> onClickTroop;
    public Action<BattleQueueElement, int> onDamageReceived;
    public Action<BattleQueueElement, int> onHealReceived;
    public Action<BattleQueueElement> onTroopDied;

    public Troop Troop { get; private set; }
    public TroopObject TroopObject { get; private set; }
    public E_BatteElementTeam Team { get; private set; }
    public List<Skill> UsedSkills { get; private set; } = new List<Skill>();
    public int DamageDuringBattle { get; private set; }

    public BattleQueueElement(Troop troop, TroopObject troopObject, E_BatteElementTeam team)
    {
        Troop = troop;
        TroopObject = troopObject;
        Team = team;

        TroopObject.onClickTroop = OnClickTroop;
        Troop.onDamageReceived = OnDamageReceived;
        Troop.onHealReceived = OnHealReceived;
        Troop.onTroopDied = OnTroopDied;

        TroopObject.UnitsCount.text = Troop.UnitsCount.ToString();
        TroopObject.ActiveImage.gameObject.SetActive(false);
    }

    public void UseSkill(Skill skill, List<BattleQueueElement> targets)
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

        skill.Use(this, targets);

        onSkillUsed?.Invoke(skill);
    }

    public void Attack(BattleQueueElement target)
    {
        int damage = 0; 
        int hpLeft = target.Troop.CurrentHealth - Troop.CurrentDamage;

        damage = hpLeft < 0 ? Troop.CurrentDamage + hpLeft : Troop.CurrentDamage;

        DamageDuringBattle += damage;

        target.Troop.DealDamage(damage);

        onPerformAttack?.Invoke(target, damage);
    }

    public void Move(Vector3 position)
    {
        TroopObject.transform.position = Vector3.MoveTowards(TroopObject.transform.position, position, GameConfig.Instance.unitSpeed);
    }

    private void OnClickTroop()
    {
        onClickTroop?.Invoke(this);
    }

    private void OnTroopDied()
    {
        onTroopDied?.Invoke(this);

        TroopObject.UnitsCount.text = Troop.UnitsCount.ToString();
    }

    private void OnDamageReceived(int damage)
    {
        onDamageReceived?.Invoke(this, damage);

        TroopObject.UnitsCount.text = Troop.UnitsCount.ToString();
    }

    private void OnHealReceived(int healAmount)
    {
        onHealReceived?.Invoke(this, healAmount);
    }

    public override bool Equals(object other)
    {
        if (other == null || !this.GetType().Equals(other.GetType()))
            return false;

        BattleQueueElement otherElement = other as BattleQueueElement;

        return Troop.UnitStats.id == otherElement.Troop.UnitStats.id && Team == otherElement.Team;
    }

    public override int GetHashCode()
    {
        int hash = 13;
        hash = hash * 7 + Troop.UnitStats.id.GetHashCode();
        hash = hash * 7 + Team.GetHashCode();

        return hash;
    }
}
