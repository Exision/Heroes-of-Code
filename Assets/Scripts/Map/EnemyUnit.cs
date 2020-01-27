using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public void RequestRandomPath()
    {
        PathfindingManager.RequestPath(transform.position, transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f)), OnPathFound);
    }

    private void OnPathFound(List<Node> path)
    {
        Path = path;
    }
}
