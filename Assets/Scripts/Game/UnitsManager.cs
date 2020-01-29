using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    [SerializeField] private UnitStats[] _units;

    public UnitStats[] Units => _units;


}
