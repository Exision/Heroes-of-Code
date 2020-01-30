using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/ActiveSkills/Heal", order = 1)]
public class HealSkill : Skill
{
    public override void Use(Troop[] targets, float usePower)
    {
        if (targets.Length > 0)
            targets[0].Heal(targets[0].UnitStats.health * Modificator);
    }
}
