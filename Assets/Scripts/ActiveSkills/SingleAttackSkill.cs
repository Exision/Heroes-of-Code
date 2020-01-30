using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleAttack", menuName = "ScriptableObject/ActiveSkills/SingleAttack", order = 1)]
public class SingleAttackSkill : Skill
{
    public override void Use(Troop[] targets, float usePower)
    {
        if (targets.Length > 0)
            targets[0].Attack(usePower * Modificator + Power);
    }
}
