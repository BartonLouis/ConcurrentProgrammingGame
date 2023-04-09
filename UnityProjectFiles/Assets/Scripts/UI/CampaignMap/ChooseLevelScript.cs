using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class ChooseLevelScript : MonoBehaviour
{
    public Transform NextButton;

    [Space(10)]
    public string levelName = "Level1";
    public int team1Players = 0;
    public int team2Players = 0;
    public int numCores = 0;
    public int minQueueTime = 0;
    public int maxQueueTime = 0;
    public int yieldBoost = 0;
    public int timeBetweenTurns = 0;
    public int numDamage = 0;
    public int numSupport = 0;
    public int numTank = 0;
    public ClassValue.ClassType[] enabledClasses;
    public TextAsset levelHint;

    // Start is called before the first frame update
    void Start()
    {
        if (NextButton == null) return;
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
        lr.SetPosition(1, new Vector3(NextButton.position.x, NextButton.position.y, 0));
    }

    public void LoadLevel()
    {
        CampaignMapController.instance.LoadLevel(levelName, team1Players, team2Players, numCores, minQueueTime, maxQueueTime, yieldBoost, timeBetweenTurns, numDamage, numSupport, numTank, enabledClasses, levelHint);
    }
}
