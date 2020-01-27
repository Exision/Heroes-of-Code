using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public Action<Vector3Int> onMoveDone;

    public void SetPath(List<Node> path)
    {
        Path = path;
    }

    public override void Move()
    {
        base.Move();

        if (Path != null)
            onMoveDone?.Invoke(Path[_nextNodeIndex - 1].TilemapPosition);
    }
}
