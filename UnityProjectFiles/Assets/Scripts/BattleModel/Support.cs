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
        BaseBoost = 0.8f;
        BaseBlock = 0.0f;

        ClassType = ClassValue.ClassType.Support;
    }
    public override void Heal(Value target)
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

        float amount = BaseTeamHeal;
        float totalMultiplier = 1;
        foreach (KeyValuePair<float, int> multipler in DamageMultipliers)
        {
            totalMultiplier += multipler.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        player.Heal(amount);
        Anim.SetTrigger("Cast");
    }

    public override void Boost(Value target)
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

        float amount = BaseBoost;
        float totalMultiplier = 1;
        foreach (KeyValuePair<float, int> multipler in DamageMultipliers)
        {
            totalMultiplier += multipler.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        player.AddDamageMultiplier(amount, BuffTime);
        Anim.SetTrigger("Cast");
    }

}
