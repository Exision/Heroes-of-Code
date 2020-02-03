using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public int Id { get; private set; }
    public List<Troop> Group { get; private set; } = new List<Troop>();
    public Vector3? Position { get; set; } = null;

    public EnemyData (int id, List<Troop> group)
    {
        Id = id;
        Group = group;
    }

    public void SetGroup(List<Troop> group)
    {
        Group = group;
    }
}
