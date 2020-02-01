using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour, ITroop
{
    public UnitStats UnitStats { get; set; }
    public int UnitsCount { get; set; }
    public int CurrentHealth { get; set; }
    public int CurrentDamage { get; set; }


    public Troop(UnitStats unitStats, int troopsCount)
    {
        UnitStats = unitStats;
        UnitsCount = troopsCount;

        CurrentHealth = UnitStats.health * UnitsCount;
        CurrentDamage = UnitStats.attackPower * UnitsCount;
    }


    public void Attack(float damage)
    {
        CurrentHealth -= Mathf.RoundToInt(damage);

        UpdateData();
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += Mathf.RoundToInt(UnitStats.health * healAmount);

        UpdateData();
    }

    private void UpdateData()
    {
        UnitsCount = Mathf.CeilToInt(CurrentHealth / UnitStats.health);
        CurrentDamage = UnitsCount * UnitStats.attackPower;
    }

    public override bool Equals(object other)
    {
        if (other == null || !this.GetType().Equals(other.GetType()))
            return false;

        return UnitStats.id == (other as Troop).UnitStats.id;
    }
}
