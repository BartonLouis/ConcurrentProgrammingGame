using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    private IDEController IDE;
    private ControlPanelManager CharacterPanel;
    private PlayControls PlayControls;

    [SerializeField] private TeamCenter Team1;
    [SerializeField] private TeamCenter Team2;
    [SerializeField] private int MinPlayers = 3;
    [SerializeField] private int MaxPlayers = 5;


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

        // Initial Setup
        IDE.Clear();
        CharacterPanel.Load(MaxPlayers);
        Team1.SetNumSpawns(MaxPlayers);
        Team2.SetNumSpawns(MaxPlayers);
        Team1.Init();
        Team2.Init();
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
        FileManager.DeleteFile(filename);
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

    public void AddPlayer(ClassValue.ClassType classType)
    {
        Team1.AddPlayer(classType);
    }

    public void UpdatePlayerClass(int index, ClassValue.ClassType classType)
    {
        Team1.UpdatePlayer(index, classType);
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
            CharacterPanel.Hide();
            PlayControls.GameStart();
        } else {
            PlayControls.Error();
        }
    }

    public void GameStop()
    {
        if (characterPanelOpen) CharacterPanel.Show();
        CharacterPanel.Load(MaxPlayers);
        PlayControls.GameStop();
    }
}
