using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/ActiveSkills/Heal", order = 1)]
public class HealSkill : Skill
{
    public override void Use(List<Troop> targets, float usePower)
    {
        if (targets.Count > 0)
            targets[0].Heal(targets[0].UnitStats.health * Modificator);
    }
}
