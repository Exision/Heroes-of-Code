using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rage", menuName = "ScriptableObject/ActiveSkills/Rage", order = 1)]
public class RageSkill : Skill
{
    public override void Use(List<Troop> targets, float usePower)
    {
        if (targets.Count > 0)
        {
            foreach (Troop target in targets)
                target.Attack(Power);
        }
    }
}
