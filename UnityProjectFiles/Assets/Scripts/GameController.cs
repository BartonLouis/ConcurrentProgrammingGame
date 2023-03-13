using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;
using System.IO;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public static int MinPlayers = 3;
    public static int MaxPlayers = 3;
    public static int Team2Players = 3;
    public static int Team2Difficulty = 1;
    public static int NumCores = 3;
    public static int MinQueueTime = 5;
    public static int MaxQueueTime = 15;
    public static int YieldBoost = 1;
    public static int PassivePriorityBoost = 1;
    public static int TimeBetweenTurns = 0;
    private System.Random Rnd;

    public enum GameState
    {
        Setup,          // For when the game is in setup phase
        Play,           // For when the game is in play mode, at regular speed
        FastPlay,       // For when the game is in fast forward mode
        Paused          // For when the battle is paused
    }

    private IDEController IDE;
    private ControlPanelManager CharacterPanel;
    private PlayControls PlayControls;
    private PauseMenuController PauseMenu;
    private BattleModel BattleModel;
    private ScheduleVisualiser ScheduleVisualiser;


    [HideInInspector] public GameState CurrentGameState { get; set; }
    [HideInInspector] public bool Paused { get; set; }

    // Setup attributes
    [SerializeField] private TeamCenter Team1;
    [SerializeField] private TeamCenter Team2;
    [Space(10)]

    // Gameplay attributes
    [SerializeField] private float PauseSpeed = 0;
    [SerializeField] private float PlaySpeed = 1;
    [SerializeField] private float FastSpeed = 2;
    [HideInInspector] public float CurrentSpeed;
    private float CurrentTime = 1;


    private bool characterPanelOpen = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        IDE = IDEController.instance;
        CharacterPanel = ControlPanelManager.instance;
        PlayControls = PlayControls.instance;
        PauseMenu = PauseMenuController.instance;
        BattleModel = BattleModel.instance;
        ScheduleVisualiser = ScheduleVisualiser.instance;
        Rnd = new System.Random();


        // Initial Setup
        IDE.Clear();
        CharacterPanel.Load(MaxPlayers);
        Team1.SetNumSpawns(MaxPlayers);
        Team2.SetNumSpawns(Team2Players);
        Team1.Init();
        Team2.Init();
        Team1.SetTeamNum(1);
        Team2.SetTeamNum(2);

        CurrentGameState = GameState.Setup;
        CurrentSpeed = PlaySpeed;
        CurrentTime = 1;

        // Populate Enemy Team
        for (int i = 0; i < Team2Players; i++)
        {
            int choice = Rnd.Next(3);
            ClassValue.ClassType playerClass;
            switch (choice)
            {
                case 0:
                    playerClass = ClassValue.ClassType.Damage;
                    break;
                case 1:
                    playerClass = ClassValue.ClassType.Support;
                    break;
                default:
                    playerClass = ClassValue.ClassType.Tank;
                    break;
            }
            Team2.AddPlayer(playerClass, playerClass.ToString() + Team2Difficulty + ".txt");

        }
    }

    public void Update()
    {
        // Check if the user has pressed the ESCAPE key to pause the game
        if (Input.GetButtonDown("Cancel") )
        {
            if (Paused)
            {
                UnPauseGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void FixedUpdate()
    {
        if (!Paused)
        {
            if (CurrentGameState != GameState.Setup)
            {
                CurrentTime -= CurrentSpeed * Time.fixedDeltaTime;
                if (CurrentTime <= 0)
                {
                    CurrentTime = 1;
                    Step();
                }
            }
        }
    }

    public void PauseGame()
    {
        Paused = true;
        PauseMenu.Show();
    }

    public void UnPauseGame()
    {
        Paused = false;
        PauseMenu.Hide();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Setup Control

    public void CreateScriptStart()
    {
        CharacterPanel.Hide();
        IDE.Clear();
        IDE.Open();
        PlayControls.IDEOpen();
    }

    public void EditScriptStart(string filename, int scriptIndex)
    {
        CharacterPanel.Hide();

        IDE.Clear();
        IDE.Open(filename, scriptIndex);
        PlayControls.IDEOpen();
    }

    public void RemoveScript(int index)
    {
        CharacterPanel.Delete(index);
        CharacterPanel.Load(MaxPlayers);
    }

    public void DeleteScript(string filename)
    {
        FileManager.DeleteFile("Player", filename);
        CharacterPanel.DeleteAll(filename);
        CharacterPanel.Load(MaxPlayers);
    }

    public void LoadScript(string filename)
    {
        CharacterPanel.Add(filename);
        CharacterPanel.Load(MaxPlayers);
    }

    public void CreateScriptComplete(string filename)
    {
        IDE.Close();
        CharacterPanel.Show();
        CharacterPanel.Add(filename);
        CharacterPanel.Load(MaxPlayers);
        PlayControls.IDEClose();
    }

    public void EditScriptComplete(string filename, int scriptIndex)
    {
        IDE.Close();
        CharacterPanel.Show();
        CharacterPanel.EditComplete(filename, scriptIndex);
        CharacterPanel.Load(MaxPlayers);
        Team1.UpdatePlayer(scriptIndex, ClassValue.ClassType.Any, filename);
        PlayControls.IDEClose();
    }
    
    public void CancelScript()
    {
        IDE.Close();
        CharacterPanel.Show();
        CharacterPanel.Load(MaxPlayers);
        PlayControls.IDEClose();
    }

    public void ExpandControlBar()
    {
        characterPanelOpen = false;
        CharacterPanel.Hide();
    }

    public void MinimiseControlBar()
    {
        characterPanelOpen = true;
        CharacterPanel.Show();
        CharacterPanel.Load(MaxPlayers);
    }

    public void AddPlayer(ClassValue.ClassType classType, string filename)
    {
        Team1.AddPlayer(classType, filename);
    }

    public void UpdatePlayerClass(int index, ClassValue.ClassType classType, string filename)
    {
        Team1.UpdatePlayer(index, classType, filename);
    }

    public void RemovePlayer(int index)
    {
        Team1.RemovePlayer(index);
    }

    // Battle Control

    public void GameStart()
    {
        if (Team1.IsFull(MinPlayers, MaxPlayers))
        {
            // Setup Battle model
            List<Character> characters = new List<Character>();
            characters.AddRange(Team1.GetCharacters());
            characters.AddRange(Team2.GetCharacters());
            ScheduleVisualiser.Setup(NumCores);
            BattleModel.StartBattle(characters.ToArray(), NumCores);

            // Setup UI elements
            CharacterPanel.Hide();
            PlayControls.GameStart();
            Pause();
        } else {
            PlayControls.Error();
        }
    }

    public void Play()
    {
        CurrentTime = 1;
        CurrentSpeed = PlaySpeed;
        CurrentGameState = GameState.Play;
    }

    public void FastForward()
    {
        CurrentTime = 1;
        CurrentSpeed = FastSpeed;
        CurrentGameState = GameState.FastPlay;
    }

    public void Pause()
    {
        CurrentTime = 1;
        CurrentSpeed = PauseSpeed;
        CurrentGameState = GameState.Paused;
    }

    public void Step()
    {
        if (CurrentGameState != GameState.Paused) CurrentGameState = GameState.Paused;
        BattleModel.Step();
        ScheduleVisualiser.Step();
    }

    public void Stop()
    {
        CurrentGameState = GameState.Setup;
        CurrentTime = 1;
        CurrentSpeed = PauseSpeed;
        if (characterPanelOpen) CharacterPanel.Show();
        CharacterPanel.Load(MaxPlayers);
        PlayControls.GameStop();
        BattleModel.EndBattle();
        ScheduleVisualiser.BattleEnd();
    }

    public void AddVisualBlock(int coreIndex, Character character, int timeSteps)
    {
        ScheduleVisualiser.AddBlock(coreIndex, character, timeSteps);
    }
    
}
