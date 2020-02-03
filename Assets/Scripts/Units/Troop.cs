using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : ITroop
{
    public Action onTroopDied;

    public UnitStats UnitStats { get; set; }
    public int UnitsCount
    {
        get => _unitCount;
        set
        {
            _unitCount = value >= 0 ? value : 0;
        }
    }
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value >= 0 ? value : 0;
        }
    }
    public int CurrentDamage
    {
        get => _currentDamage;
        set
        {
            _currentDamage = value >= 0 ? value : 0;
        }
    }

    private int _unitCount;
    private int _currentHealth;
    private int _currentDamage;

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

        if (CurrentHealth <= 0)
            onTroopDied?.Invoke();

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

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + UnitStats.id.GetHashCode();

        return hash;
    }
}
