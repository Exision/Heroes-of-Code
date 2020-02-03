using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] protected int _id;
    [SerializeField] protected float _usePower;
    [SerializeField] protected int _skillPower;
    [SerializeField] protected E_SkillUsageTarget _skillTarget;

    public int Id => _id;
    public float Modificator => _usePower;
    public int Power => _skillPower;
    public E_SkillUsageTarget SkillTarget => _skillTarget;

    public abstract void Use(List<BattleQueueElement> targets, float usePower);

    public enum E_SkillUsageTarget
    {
        All,
        Enemy,
        Player
    }
}
