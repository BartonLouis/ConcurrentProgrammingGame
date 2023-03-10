using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpreter;

public class BattleModel : MonoBehaviour
{
    public static BattleModel instance;

    public int CurrentTimeStep;
    private Character[] Characters;
    private Core[] Cores;
    private ThreadScheduler ThreadScheduler;
    private GameController Controller;

    [SerializeField] private int MinQueueTime = 5;
    [SerializeField] private int MaxQueueTime = 15;
    [SerializeField] private int ForecastSize = 50;
    [SerializeField] private int YieldBoost = 1;
    [SerializeField] private int PassivePriorityBoost = 1;

    private System.Random Rnd;

    public void Awake()
    {
        instance = this;
        Rnd = new System.Random();
    }

    public void Start()
    {
        Controller = GameController.instance;
    }

    public void StartBattle(Character[] characters, int numCores)
    {
        CurrentTimeStep = 0;
        Characters = characters;
        foreach (Character character in Characters)
        {
            character.Setup();
        }

        Cores = new Core[numCores];
        for (int i = 0; i < numCores; i++)
        {
            Cores[i] = new Core(i);
        }

        ThreadScheduler = new ThreadScheduler(Characters, Cores, MinQueueTime, MaxQueueTime, ForecastSize, YieldBoost, PassivePriorityBoost);
    }

    public void EndBattle()
    {
        foreach (Character character in Characters)
        {
            character.OnGameEnd();
        }
        CurrentTimeStep = 0;
    }

    public void Step()
    {
        // Loop through each core and take a step
        Core[] cores = ThreadScheduler.GetCores();
        foreach(Core core in cores)
        {
            core.Step(CurrentTimeStep);
        }
        CurrentTimeStep++;
        // Make the thread scheduler take a step
        foreach(Character c in Characters)
        {
            c.EndOfStepUpdate();
        }
        ThreadScheduler.Step(CurrentTimeStep);
    }

    public void AddVisualBlock(int coreIndex, Character character, int timeQueued)
    {
        Controller.AddVisualBlock(coreIndex, character, timeQueued);
    }

    // Public functions used to get information about the game

    public IntValue GetHealth(Character character)
    {
        return new IntValue(Mathf.FloorToInt(character.GetHealth()));
    }

    public IntValue GetMaxHealth(Character character)
    {
        return new IntValue(Mathf.FloorToInt(character.GetMaxHealth()));
    }

    public BoolValue IsMaxHealth(Character character)
    {
        return new BoolValue(character.IsMaxHealth());
    }
    
    public PlayerValue GetEnemyOfType(Character character, Value playerClass){
        if (playerClass == null) return null;
        ClassValue classValue = playerClass.GetAsClass();
        if (classValue == null) return null;

        // Logic to get a player reference
        List<Character> choices = new List<Character>();
        foreach(Character c in Characters)
        {
            // If Character is alive and is on the same team and isn't the same as the character requesting a teammate and they have the requested class
            if (c.IsAlive() && c.Team != character.Team && c.ClassType == classValue.Value)
            {
                choices.Add(c);
            }
        }
        // Choose a random character from choices list or return null if empty list
        if (choices.Count == 0) return null;
        int index = Rnd.Next(choices.Count);
        return new PlayerValue(choices[index]);
    }

    public PlayerValue GetTeammateOfType(Character character, Value playerClass)
    {
        if (playerClass == null) return null;
        ClassValue classValue = playerClass.GetAsClass();
        if (classValue == null) return null;

        // Logic to get an enemy of the desired class
        List<Character> choices = new List<Character>();
        foreach(Character c in Characters)
        {
            if (c.IsAlive() && c.Team == character.Team && c != character && c.ClassType == classValue.Value)
            {
                choices.Add(c);
            }
        }
        // Choose a random character from choices list or return null if empty list
        if (choices.Count == 0) return null;
        int index = Rnd.Next(choices.Count);
        return new PlayerValue(choices[index]);
    }

    public BoolValue IsCharged(Value player)
    {
        if (player == null) return null;
        PlayerValue playerValue = player.GetAsPlayer();
        if (playerValue == null) return null;

        // Logic to check if that player is charged or not
        return new BoolValue(playerValue.PlayerRef.IsCharged());
    }

    public BoolValue IsNone(Value variable)
    {
        return new BoolValue(variable == null);
    }

    public BoolValue IsNotNone(Value variable)
    {
        return new BoolValue(variable != null);
    }

    public PlayerValue GetPlayerComponent(Value message)
    {
        if (message == null) return null;
        MessageValue messageValue = message.GetAsMessage();
        if (messageValue == null) return null;

        return messageValue.PlayerComponent;
    }

    public StringValue GetTextComponent(Value message)
    {
        if (message == null) return null;
        MessageValue messageValue = message.GetAsMessage();
        if (messageValue == null) return null;

        return messageValue.StringComponent;
    }

    public ClassValue GetClass(Value player)
    {
        if (player == null) return null;
        PlayerValue playerValue = player.GetAsPlayer();
        if (playerValue == null) return null;

        return new ClassValue(playerValue.PlayerRef.ClassType);
    }

}
