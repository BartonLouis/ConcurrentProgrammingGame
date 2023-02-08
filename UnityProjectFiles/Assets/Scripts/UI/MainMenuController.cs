using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    private Animator MainMenuAnimator;
    private Animator SettingsAnimator;


    private void Start()
    {
        MainMenuAnimator = GameObject.Find("Main Menu").GetComponent<Animator>();
        SettingsAnimator = GameObject.Find("Settings Menu").GetComponent<Animator>();
    }

    public void Campaign()
    {
        Debug.Log("Campaign Button Pressed");
        AudioManager.instance.Play("MenuNavigate");
        SceneManager.LoadScene(1);
    }

    public void Skirmish()
    {
        Debug.Log("Skirmish Button Pressed");
        AudioManager.instance.Play("MenuNavigate");
        SceneManager.LoadScene(1);
    }

    public void SettingsOpen()
    {
        Debug.Log("Settings Button Pressed");
        AudioManager.instance.Play("MenuNavigate");
        MainMenuAnimator.SetBool("Open", false);
        SettingsAnimator.SetBool("Open", true);
    }

    public void SettingsClose()
    {
        Debug.Log("Settings Back Button Pressed");
        AudioManager.instance.Play("Back");
        MainMenuAnimator.SetBool("Open", true);
        SettingsAnimator.SetBool("Open", false);
    }

    public void Quit()
    {
        AudioManager.instance.Play("Back");
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }
}
