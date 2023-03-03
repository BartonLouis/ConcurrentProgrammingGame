using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : MonoBehaviour
{
    public static BattleModel instance;

    public int CurrentTimeStep;
    private Character[] Characters;

    public void Awake()
    {
        instance = this;
    }

    public void StartBattle(Character[] characters)
    {
        CurrentTimeStep = 0;
        Characters = characters;
        foreach (Character character in Characters)
        {
            character.Setup();
        }
    }

    public void EndBattle()
    {
        CurrentTimeStep = 0;
    }

    public void Step()
    {
        // Temporary implementation, all characters take a turn
        foreach(Character character in Characters){
            character.Step();
        }
    }

}
