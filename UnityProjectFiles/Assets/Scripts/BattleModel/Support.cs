using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class Support : Character
{
    void Start()
    {
        BaseMaxHealth = 80;
        BaseDamage = 15;
        BaseSelfHeal = 30;
        BaseTeamHeal = 40;
        BaseSelfDefend = 0.3f;
        BaseTeamDefend = 0.0f;
        BaseBoost = 0.25f;
        BaseBlock = 0.0f;

        ClassType = ClassValue.ClassType.Support;
    }
}
