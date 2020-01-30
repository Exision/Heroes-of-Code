using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaAttack", menuName = "ScriptableObject/ActiveSkills/AreaAttack", order = 1)]
public class AreaAttackSkill : Skill
{
    public override void Use(Troop[] targets, float usePower)
    {
        if (targets.Length > 0)
        {
            foreach (Troop target in targets)
                target.Attack(usePower * Modificator);
        }
    }
}
