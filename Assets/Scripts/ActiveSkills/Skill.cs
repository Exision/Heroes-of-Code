using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] protected int _id;
    [SerializeField] protected float _usePower;
    [SerializeField] protected int _skillPower;
    [SerializeField] protected E_SkillUsageTarget _skillTarget;
    [SerializeField] protected bool _isMultiTarget;

    public int Id => _id;
    public float Modificator => _usePower;
    public int Power => _skillPower;
    public E_SkillUsageTarget SkillTarget => _skillTarget;
    public bool IsMultiTarget => _isMultiTarget;

    public abstract void Use(BattleQueueElement performer, List<BattleQueueElement> targets);

    public enum E_SkillUsageTarget
    {
        All,
        Enemy,
        Ally
    }
}
