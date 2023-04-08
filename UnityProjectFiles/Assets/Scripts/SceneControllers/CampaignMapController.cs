using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Interpreter;

public class CampaignMapController : MonoBehaviour
{

    public static CampaignMapController instance;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        AudioManager.instance.PlayMusic("SkirmishSetup");
    }
    public void MainMenu()
    {
        AudioManager.instance.Play("Menu2");
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel(string levelName, int team1Players, int team2Players, int numCores, int minQueueTime, int maxQueueTime, int yieldBoost, int timeBetweenTurns, int numDamage, int numSupport, int numTank, ClassValue.ClassType[] enabledClasses)
    {
        Debug.Log("Loading Level: " + levelName);
        AudioManager.instance.Play("Menu1");
        GameController.level = levelName;
        GameController.MinPlayers = team1Players;
        GameController.MaxPlayers = team1Players;
        GameController.Team2Players = team2Players;
        GameController.NumCores = numCores;
        GameController.MinQueueTime = minQueueTime;
        GameController.MaxQueueTime = maxQueueTime;
        GameController.YieldBoost = yieldBoost;
        GameController.TimeBetweenTurns = timeBetweenTurns;
        GameController.numDamage = numDamage;
        GameController.numSupport = numSupport;
        GameController.numTank = numTank;
        GameController.enabledClasses = enabledClasses;
        SceneManager.LoadScene("Battle");
    }

}
