using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpreter;
using TMPro;

public class BattleModel : MonoBehaviour
{
    public static BattleModel instance;

    public int CurrentTimeStep;
    private Character[] Characters;
    private Core[] Cores;
    private ThreadScheduler ThreadScheduler;
    private GameController Controller;

    [SerializeField] private int ForecastSize;

    private int MinQueueTime ;
    private int MaxQueueTime;
    private int YieldBoost;
    private int PassivePriorityBoost;
    private int MinTimeBetweenTurns;

    private bool shouldReschedule = false;
    private bool started = false;
    private System.Random Rnd;

    private TeamCenter Team1;
    private TeamCenter Team2;

    public void Awake()
    {
        instance = this;
        Rnd = new System.Random();
    }

    public void Start()
    {
        Controller = GameController.instance;
        MinQueueTime = GameController.MinQueueTime;
        MaxQueueTime = GameController.MaxQueueTime;
        YieldBoost = GameController.YieldBoost;
        PassivePriorityBoost = GameController.PassivePriorityBoost;
        MinTimeBetweenTurns = GameController.TimeBetweenTurns;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && started)
        {
            foreach(Character c in Characters)
            {
                c.Damage(1000);
            }
            Step();
        }
    }

    public void StartBattle(Character[] characters, int numCores, TeamCenter t1, TeamCenter t2)
    {
        started = true;
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
        Team1 = t1;
        Team2 = t2;
        ThreadScheduler = new ThreadScheduler(Characters, Cores, MinQueueTime, MaxQueueTime, ForecastSize, YieldBoost, PassivePriorityBoost, MinTimeBetweenTurns);
        
    }

    public void SetShouldReschedule()
    {
        shouldReschedule = true;
        ThreadScheduler.SetShouldReschedule();
    }

    public void SetShouldReschedule(TeamCenter team)
    {
        shouldReschedule = true;
        ThreadScheduler.SetShouldReschedule(team);
    }

    public void EndBattle()
    {
        foreach (Character character in Characters)
        {
            character.OnGameEnd();
        }
        CurrentTimeStep = 0;
        started = false;
    }

    public void Step()
    {
        // Loop through each core and take a step
        Core[] cores = ThreadScheduler.GetCores();
        foreach(Core core in cores)
        {
            core.Step(CurrentTimeStep);
        }
        // Make the thread scheduler take a step
        foreach(Character c in Characters)
        {
            if (c.IsAlive())
            {
                c.EndOfStepUpdate();
            }
        }
        ThreadScheduler.Step(CurrentTimeStep);

        if (Team1.AllDead())
            Controller.GameOver(Team2);
        else if (Team2.AllDead())
            Controller.GameOver(Team1);
        
        if (shouldReschedule)
        {
            List<List<KeyValuePair<Character, int>>> representation = ThreadScheduler.GetVisualRepresentation();
            Controller.ResetScheduleVisualiser(representation);
        }
        shouldReschedule = false;
        CurrentTimeStep++;
        
    }

    public void RemoveCharacter(Character character)
    {
        ThreadScheduler.RemoveCharacter(character);
    }

    public void AddVisualBlock(int coreIndex, Character character, int timeQueued)
    {
        if (!shouldReschedule)
        {
            Controller.AddVisualBlock(coreIndex, character, timeQueued);
        }
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
        
        if (playerClass == null)
        {
            return null;
        }
        ClassValue classValue = playerClass.GetAsClass();

        if (classValue == null) {
            return null;
        }

        // Logic to get a player reference
        List<Character> choices = new List<Character>();
        foreach(Character c in Characters)
        {
            // If Character is alive and is on the same team and isn't the same as the character requesting a teammate and they have the requested class
            if (c.IsAlive() && c.Team != character.Team && (c.ClassType == classValue.Value || classValue.Value == ClassValue.ClassType.Any))
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
            if (c.IsAlive() && c.Team == character.Team && c != character && (c.ClassType == classValue.Value || classValue.Value == ClassValue.ClassType.Any))
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
        if (variable == null) return new BoolValue(false);
        try
        {
            Character c = variable.GetAsPlayer().PlayerRef;
            return new BoolValue(c.IsAlive());
        } catch
        { 
            return new BoolValue(variable != null);
        }

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

    public void SendMessageToAll(TeamCenter team, MessageValue message)
    {
        foreach (Character c in Characters)
        {
            if (c.IsAlive() && c.Team == team)
            {
                c.ReceiveMessage(message);
            }
        }
    }

    public int GetNumberOfTeammates(Character character)
    {
        int teammates = 0;
        foreach (Character c in Characters)
        {
            if (c.IsAlive() && c != character && c.Team == character.Team)
            {
                teammates++;
            }
        }
        return teammates;
    }

}
