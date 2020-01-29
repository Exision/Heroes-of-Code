using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSingleTargetAttack : ISkillUse
{
    public void Use(Troop[] targets, float skillPower)
    {
        if (targets.Length > 0)
            targets[0].Attack(skillPower);
    }
}
