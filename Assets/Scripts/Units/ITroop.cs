public interface ITroop
{
    UnitStats UnitStats { get; set; }

    int UnitsCount { get; set; }
    int CurrentHealth { get; set; }
    int CurrentDamage { get; set; }

    void DealDamage(float damage);
    void Heal(float healAmount);
}
