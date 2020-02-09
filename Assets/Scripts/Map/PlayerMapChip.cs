using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapChip : MapChip
{
    public Action<Vector3Int, Vector3> onMoveDone;

    public void SetPath(List<Node> path)
    {
        Path = path;
    }

    public override void Move()
    {
        if (Path != null)
            onMoveDone?.Invoke(Path[_nextNodeIndex].TilemapPosition, Path[_nextNodeIndex].WorldPosition);

        base.Move();
    }
}
