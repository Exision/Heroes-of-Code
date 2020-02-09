using System;
using UnityEngine;

public class Troop : ITroop
{
    public Action<int> onDamageReceived;
    public Action<int> onHealReceived;
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


    public void DealDamage(float damage)
    {
        CurrentHealth -= Mathf.RoundToInt(damage);

        UpdateData();

        onDamageReceived?.Invoke(Mathf.RoundToInt(damage));

        if (CurrentHealth <= 0)
            onTroopDied?.Invoke();
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += Mathf.RoundToInt(healAmount);

        UpdateData();

        onHealReceived?.Invoke(Mathf.RoundToInt(healAmount));
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
