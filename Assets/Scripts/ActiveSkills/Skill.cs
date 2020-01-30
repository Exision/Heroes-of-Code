using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] protected int _id;
    [SerializeField] protected float _usePower;
    [SerializeField] protected int _skillPower;

    public int Id => _id;
    public float Modificator => _usePower;
    public int Power => _skillPower;

    public abstract void Use(Troop[] targets, float usePower);
}
