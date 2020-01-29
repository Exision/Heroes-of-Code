public interface ISkill
{
    float UsePowerModificator { get; }

    void Use(Troop[] targets, float skillPowerModifie);
}
