using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayControls : MonoBehaviour
{

    public static PlayControls instance;

    [SerializeField] Sprite UpIcon;
    [SerializeField] Sprite DownIcon;
    [SerializeField] Image ExpandButton;

    [SerializeField] Image PlayButton;
    [SerializeField] Image PauseButton;
    [SerializeField] Image Speed1Button;
    [SerializeField] Image Speed2Button;
    [SerializeField] Image Speed3Button;

    private bool Expanded = true;

    private Animator Anim;
    private GameController Controller;


    private void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        Controller = GameController.instance;
    }
    public void ExpandButtonPressed()
    {
        Expanded = !Expanded;
        if (Expanded)
        {
            ExpandButton.sprite = DownIcon;
            Controller.MinimiseControlBar();
        }
        else
        {
            ExpandButton.sprite = UpIcon;
            Controller.ExpandControlBar();
        }
    }

    public void PlayButtonPressed()
    {
        PlayButton.color = Color.grey;
        PauseButton.color = Color.white;
        Controller.Play();
    }

    public void PauseButtonPressed()
    {
        PlayButton.color = Color.white;
        PauseButton.color = Color.grey;
        Controller.Pause();
    }


    public void StepButtonPressed()
    {
        PlayButton.color = Color.white;
        PauseButton.color = Color.grey;
        Controller.Pause();
        Controller.StepClicked();
    }

    public void StopButtonPressed()
    {
        PlayButton.color = Color.white;
        PauseButton.color = Color.white;
        Controller.Stop();
    }

    public void BeginGamePressed()
    {
        Controller.GameStart();
    }

    public void Speed1Pressed()
    {
        Controller.SetSpeed(1);
        Speed1Button.color = Color.grey;
        Speed2Button.color = Color.white;
        Speed3Button.color = Color.white;
    }

    public void Speed2Pressed()
    {
        Controller.SetSpeed(2);
        Speed1Button.color = Color.white;
        Speed2Button.color = Color.grey;
        Speed3Button.color = Color.white;
    }

    public void Speed3Pressed()
    {
        Controller.SetSpeed(3);
        Speed1Button.color = Color.white;
        Speed2Button.color = Color.white;
        Speed3Button.color = Color.grey;
    }


    public void GameStart()
    {
        PlayButton.color = Color.white;
        PauseButton.color = Color.grey;
        Speed1Button.color = Color.grey;
        Speed2Button.color = Color.white;
        Speed3Button.color = Color.white;
        Controller.SetSpeed(1);
        Anim.SetTrigger("GameStart");
    }

    public void GameStop()
    {
        PlayButton.color = Color.white;
        PauseButton.color = Color.white;
        Anim.SetTrigger("GameEnd");
    }

    public void IDEOpen()
    {
        Anim.SetBool("IDEOpen", true);
    }

    public void IDEClose()
    {
        Anim.SetBool("IDEOpen", false);
    }
    public void Error()
    {
        Anim.SetTrigger("Error");
    }
}
