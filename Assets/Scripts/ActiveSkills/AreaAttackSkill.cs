using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaAttack", menuName = "ScriptableObject/ActiveSkills/AreaAttack", order = 1)]
public class AreaAttackSkill : Skill
{
    public override void Use(List<Troop> targets, float usePower)
    {
        if (targets.Count > 0)
        {
            foreach (Troop target in targets)
                target.Attack(usePower * Modificator);
        }
    }
}
