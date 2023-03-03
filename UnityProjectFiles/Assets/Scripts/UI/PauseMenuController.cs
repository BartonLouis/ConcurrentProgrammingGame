using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{

    public static PauseMenuController instance;

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

    public void Show()
    {
        Anim.SetBool("Open", true);
    } 

    public void Hide()
    {
        Anim.SetBool("Open", false);
    }

    public void PauseButtonPressed()
    {
        Controller.PauseGame();
    }

    public void UnPauseButtonPressed()
    {
        Controller.UnPauseGame();
    }

    public void ExitButtonPressed()
    {
        Controller.MainMenu();
    }
}
