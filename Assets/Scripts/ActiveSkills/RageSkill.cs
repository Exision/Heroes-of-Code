using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rage", menuName = "ScriptableObject/ActiveSkills/Rage", order = 1)]
public class RageSkill : Skill
{
    public override void Use(Troop[] targets, float usePower)
    {
        if (targets.Length > 0)
        {
            foreach (Troop target in targets)
                target.Attack(Power);
        }
    }
}
