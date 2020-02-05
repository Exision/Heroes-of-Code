using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObject/ActiveSkills/Heal", order = 1)]
public class HealSkill : Skill
{
    public override void Use(BattleQueueElement performer, List<BattleQueueElement> targets)
    {
        if (targets.Count > 0)
            targets[0].Troop.Heal(targets[0].Troop.UnitStats.health * Modificator);
    }
}
