using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class Tank : Character
{
    void Start()
    {
        BaseMaxHealth = 150;
        BaseDamage = 20;
        BaseSelfHeal = 15;
        BaseTeamHeal = 0;
        BaseSelfDefend = 0.4f;
        BaseTeamDefend = 0.4f;
        BaseBoost = 0.0f;
        BaseBlock = 0.2f;

        ClassType = ClassValue.ClassType.Tank;
    }
}
