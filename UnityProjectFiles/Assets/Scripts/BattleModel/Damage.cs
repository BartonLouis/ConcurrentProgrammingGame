using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class Damage : Character
{
    void Start()
    {
        BaseMaxHealth = 100;
        BaseDamage = 25;
        BaseSelfHeal = 10;
        BaseTeamHeal = 0;
        BaseSelfDefend = 0.3f;
        BaseTeamDefend = 0.0f;
        BaseBoost = 0.0f;
        BaseBlock = 0.0f;

        ClassType = ClassValue.ClassType.Damage;
    }
}
