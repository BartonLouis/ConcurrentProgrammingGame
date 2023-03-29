using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLibrary : MonoBehaviour
{

    public static PrefabLibrary instance;
    // Characters
    public GameObject DamagePrefab;
    public GameObject SupportPrefab;
    public GameObject TankPrefab;

    [Space(10)]
    // Character UI Elements
    public GameObject EnergyBarPrefab;
    public GameObject HealthBarPrefab;
    public GameObject CharacterBasePrefab;
    public GameObject DamageTextPrefab;
    public GameObject BuffStackPrefab;
    public GameObject DebuffStackPrefab;
    public GameObject DefenseBuffStackPrefab;

    [Space(10)]
    // Particle Effects
    public GameObject GetHitParticlePrefab;
    public GameObject AttackParticlePrefab;
    public GameObject ChargedParticlePrefab;

    [Space(10)]
    // Sub-UI Elements
    public GameObject EnergyBlockPrefab;
    public GameObject BuffPrefab;
    public GameObject DebuffPrefab;
    public GameObject ShieldPrefab;

    [Space(10)]
    // Misc
    public GameObject EmptySlotPrefab;
    public GameObject ChargePointPrefab;

    private void Awake()
    {
        instance = this;
    }
}