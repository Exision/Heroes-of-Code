using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleAttack", menuName = "ScriptableObject/ActiveSkills/SingleAttack", order = 1)]
public class SingleAttackSkill : Skill
{
    public override void Use(List<BattleQueueElement> targets, float usePower)
    {
        if (targets.Count > 0)
            targets[0].Troop.Attack(usePower * Modificator + Power);
    }
}
