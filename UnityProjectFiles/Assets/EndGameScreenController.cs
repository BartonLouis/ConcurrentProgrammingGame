using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGameScreenController : MonoBehaviour
{

    public static EndGameScreenController instance;

    [SerializeField] TextMeshProUGUI TitleText;
    [SerializeField] TextMeshProUGUI TopButtonText;
    [SerializeField] TextMeshProUGUI BottomButtonText;
    [Space(10)]
    [SerializeField] Color VictoryColour = new Color(0, 0, 1);
    [SerializeField] Color DefeatColour = new Color(1, 0, 0);

    Animator Animator;
    GameController Controller;
    private bool victory = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        Controller = GameController.instance;
    }

    private void Victory()
    {
        victory = true;
        TitleText.text = "Victory!";
        TitleText.color = VictoryColour;
        TopButtonText.text = "Next";
        BottomButtonText.text = "Exit";
    }

    private void Defeat()
    {
        victory = false;
        TitleText.text = "Defeat!";
        TitleText.color = DefeatColour;
        TopButtonText.text = "Retry";
        BottomButtonText.text = "Exit";
    }

    public void Show(bool victory)
    {
        if (victory) Victory();
        else Defeat();
        Animator.SetBool("Open", true);
    }

    public void Hide() { 
    
        Animator.SetBool("Open", false);
    }

    public void Button1Pressed()
    {
        if (victory)
            Controller.EndGameNext();
        else
            Controller.EndGameRetry();
    }

    public void Button2Pressed()
    {
        Controller.MainMenu();
    }


}
