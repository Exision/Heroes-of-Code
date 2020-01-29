using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSkill", menuName = "ScriptableObject/ActiveSkill", order = 1)]
public class Skill : ScriptableObject, ISkill
{
    [SerializeField] private int _id;
    [SerializeField] private float _usePower;
    [SerializeField] private ISkillUse _skillUseType;

    public float UsePowerModificator => _usePower;


    public void Use(Troop[] targets, float usePower)
    {
        _skillUseType.Use(targets, usePower);
    }
}
