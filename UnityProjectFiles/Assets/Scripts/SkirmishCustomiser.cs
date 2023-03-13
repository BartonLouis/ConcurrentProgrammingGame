using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SkirmishCustomiser : MonoBehaviour
{
    public TextMeshProUGUI Team1PlayersText;
    public TextMeshProUGUI Team2PlayersText;
    public TextMeshProUGUI Team2DifficultyText;
    public TextMeshProUGUI NumCoresText;
    public TextMeshProUGUI MinQueueTimeText;
    public TextMeshProUGUI MaxQueueTimeText;
    public TextMeshProUGUI YieldBoostText;
    public TextMeshProUGUI TimeBetweenTurnsText;

    [Space(10)]

    public Slider Team1PlayerSlider;
    public Slider Team2PlayerSlider;
    public Slider Team2DifficultSlider;
    public Slider NumCoresSlider;
    public Slider MinQueueTimeSlider;
    public Slider MaxQueueTimeSlider;
    public Slider YieldBoostSlider;
    public Slider TimeBetweenTurnsSlider;

    [Space(10)]

    private int Team1Players = 5;
    private int Team2Players = 5;
    private int Team2Difficulty = 1;
    private int NumCores = 3;
    private int MinQueueTime = 10;
    private int MaxQueueTime = 20;
    private int YieldBoost = 3;
    private int TimeBetweenTurns = 2;

    public void Start()
    {
        Team1PlayersChanged();
        Team2PlayersChanged();
        Team2DifficultyChanged();
        NumCoresChanged();
        MinQueueTimeChanged();
        MaxQueueTimeChanged();
        YieldBoostChanged();
        TimeBetweenTurnsChanged();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        GameController.MinPlayers = Team1Players;
        GameController.MaxPlayers = Team1Players;
        GameController.Team2Players = Team2Players;
        GameController.Team2Difficulty = Team2Difficulty;
        GameController.NumCores = NumCores;
        GameController.MinQueueTime = MinQueueTime;
        GameController.MaxQueueTime = MaxQueueTime;
        GameController.YieldBoost = YieldBoost;
        GameController.TimeBetweenTurns = TimeBetweenTurns;
        SceneManager.LoadScene("Battle");
    }

    public void Team1PlayersChanged()
    {
        Team1Players = Mathf.FloorToInt(Team1PlayerSlider.value);
        Team1PlayersText.text = "Number of Players: " + Team1Players;
    }

    public void Team2PlayersChanged()
    {
        Team2Players = Mathf.FloorToInt(Team2PlayerSlider.value);
        Team2PlayersText.text = "Number of Players: " + Team2Players;
    }

    public void Team2DifficultyChanged()
    {
        Team2Difficulty = Mathf.FloorToInt(Team2DifficultSlider.value);
        Team2DifficultyText.text = "Difficulty: " + Team2Difficulty;
    }
    

    public void NumCoresChanged()
    {
        NumCores = Mathf.FloorToInt(NumCoresSlider.value);
        NumCoresText.text = "Number of Cores: " + NumCores;
    }

    public void MinQueueTimeChanged()
    {
        MinQueueTime = Mathf.FloorToInt(MinQueueTimeSlider.value);
        MaxQueueTime = Mathf.Max(MaxQueueTime, MinQueueTime+1);
        MaxQueueTimeSlider.value = MaxQueueTime;
        MinQueueTimeText.text = "Min Queue Time: " + MinQueueTime;
        MaxQueueTimeText.text = "Max Queue Time: " + MaxQueueTime;
    }

    public void MaxQueueTimeChanged()
    {
        MaxQueueTime = Mathf.FloorToInt(MaxQueueTimeSlider.value);
        MinQueueTime = Mathf.Min(MinQueueTime, MaxQueueTime - 1);
        MinQueueTimeSlider.value = MinQueueTime;
        MaxQueueTimeText.text = "Max Queue Time: " + MaxQueueTime;
        MinQueueTimeText.text = "Min Queue Time: " + MinQueueTime;

    }

    public void YieldBoostChanged() {
        YieldBoost = Mathf.FloorToInt(NumCoresSlider.value);
        YieldBoostText.text = "Yield Boost: " + YieldBoost;
    }

    public void TimeBetweenTurnsChanged()
    {
        TimeBetweenTurns = Mathf.FloorToInt(TimeBetweenTurnsSlider.value);
        TimeBetweenTurnsText.text = "Time Between Turns:  " + TimeBetweenTurns;
    }
    
}
