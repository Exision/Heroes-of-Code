using UnityEngine;

public class UnitsStorage : MonoBehaviour
{
    [SerializeField] private UnitStats[] _units;

    public UnitStats[] Units => _units;
}
