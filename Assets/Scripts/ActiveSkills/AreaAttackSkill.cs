using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaAttack", menuName = "ScriptableObject/ActiveSkills/AreaAttack", order = 1)]
public class AreaAttackSkill : Skill
{
    public override void Use(BattleQueueElement performer, List<BattleQueueElement> targets)
    {
        if (targets.Count > 0)
        {
            foreach (BattleQueueElement target in targets)
                target.Troop.DealDamage(Power);
        }
    }
}
