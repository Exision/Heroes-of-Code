using UnityEngine;

[CreateAssetMenu(fileName = "UnitStat", menuName = "ScriptableObject/UnitStat", order = 1)]
public class UnitStats : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public int health;
    [SerializeField] public int attackPower;
    [SerializeField] public Skill[] skills;
}
