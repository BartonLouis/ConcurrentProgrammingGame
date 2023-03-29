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
        BaseSelfDefend = 0.6f;
        BaseTeamDefend = 0.6f;
        BaseBoost = 0.0f;
        BaseBlock = -0.5f;

        ClassType = ClassValue.ClassType.Tank;
    }

    public override void Defend(Value target)
    {
        Debug.Log("Here " + target);
        Character player;
        try
        {
            player = target.GetAsPlayer().PlayerRef;
        }
        catch
        {
            return;
        }
        Debug.Log("Defending " + player);

        float amount = BaseTeamDefend;
        float totalMultiplier = 1;
        foreach (KeyValuePair<float, int> multiplier in DamageMultipliers)
        {
            totalMultiplier += multiplier.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        amount *= getChargedMultiplier();
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
        foreach (KeyValuePair<float, int> multiplier in DamageMultipliers)
        {
            totalMultiplier += multiplier.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        amount *= getChargedMultiplier();
        player.AddDamageMultiplier(amount, BuffTime);
        Anim.SetTrigger("Cast");
    }
}
