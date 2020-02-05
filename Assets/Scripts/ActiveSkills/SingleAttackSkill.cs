using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleAttack", menuName = "ScriptableObject/ActiveSkills/SingleAttack", order = 1)]
public class SingleAttackSkill : Skill
{
    public override void Use(BattleQueueElement performer, List<BattleQueueElement> targets)
    {
        if (targets.Count > 0)
            targets[0].Troop.DealDamage(performer.DamageDuringBattle * Modificator + Power);
    }
}
