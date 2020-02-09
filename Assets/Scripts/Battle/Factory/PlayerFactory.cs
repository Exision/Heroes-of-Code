using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : ITroopObjectsFactory
{
    public TroopObject[] Create(List<Troop> troops)
    {
        List<TroopObject> createdTroops = new List<TroopObject>();

        if (troops.Count <= 0)
            return null;

        for (int loop = 0; loop < troops.Count; loop++)
        {
            TroopObject tObject = MonoBehaviour.Instantiate<TroopObject>(Resources.Load<TroopObject>($"{GameConfig.Instance.troopsPrefabsPath}Troop_{troops[loop].UnitStats.id}"));

            tObject.transform.position = GetPosition(loop);

            createdTroops.Add(tObject);
        }

        return createdTroops.ToArray();
    }

    private Vector3 GetPosition(int index)
    {
        return GameConfig.Instance.basePlayerGroupPosition +
            GameConfig.Instance.groupPositionOffset *
            Mathf.Cos(index % 2 == 0 ? 0 : Mathf.PI) * Mathf.CeilToInt((float)index / 2f);
    }
}
