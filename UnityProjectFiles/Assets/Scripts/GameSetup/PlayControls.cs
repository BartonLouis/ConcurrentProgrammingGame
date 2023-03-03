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
        Debug.Log("Play Button Pressed");
        Controller.Play();
    }

    public void PauseButtonPressed()
    {
        Debug.Log("PauseButtonPressed");
        Controller.Pause();
    }


    public void StepButtonPressed()
    {
        Debug.Log("StepButtonPressed");
        Controller.Step();
    }

    public void FastButtonPressed()
    {
        Debug.Log("FastButtonPressed");
        Controller.FastForward();
    }

    public void StopButtonPressed()
    {
        Controller.Stop();
    }

    public void BeginGamePressed()
    {
        Controller.GameStart();
    }

    public void GameStart()
    {
        Anim.SetTrigger("GameStart");
    }

    public void GameStop()
    {
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
