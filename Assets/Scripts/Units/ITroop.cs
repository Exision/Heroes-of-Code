public interface ITroop
{
    UnitStats UnitStats { get; set; }

    int UnitsCount { get; set; }
    int CurrentHealth { get; set; }
    int CurrentDamage { get; set; }

    void Attack(float damage);
    void Heal(float healAmount);
}
