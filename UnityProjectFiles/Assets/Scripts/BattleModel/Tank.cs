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
        BaseBlock = -0.2f;

        ClassType = ClassValue.ClassType.Tank;
    }

    public override void Defend(Value target)
    {
        Character player;
        try
        {
            player = target.GetAsPlayer().PlayerRef;
        }
        catch
        {
            return;
        }

        float amount = BaseTeamDefend;
        float totalMultiplier = 1;
        Debug.Log(this + "Defending Self");
        foreach (KeyValuePair<float, int> multiplier in DamageMultipliers)
        {
            totalMultiplier += multiplier.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        AddDefenseMultiplier(amount, 10);
        player.AddDefenseMultiplier(amount, BuffTime);
        Anim.SetTrigger("Cast");
    }

    public override void Block(Value target)
    {
        Character player;
        try
        {
            player = target.GetAsPlayer().PlayerRef;
        }
        catch
        {
            return;
        }
        float amount = BaseBlock;
        float totalMultiplier = 1;
        Debug.Log(this + "Defending Self");
        foreach (KeyValuePair<float, int> multiplier in DamageMultipliers)
        {
            totalMultiplier += multiplier.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        AddDefenseMultiplier(amount, 10);
        player.AddDamageMultiplier(amount, BuffTime);
        Anim.SetTrigger("Cast");
    }
}
