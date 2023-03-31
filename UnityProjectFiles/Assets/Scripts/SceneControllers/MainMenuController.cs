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
        AudioManager.instance.PlayMusic("MainMenu");
    }

    public void Campaign()
    {
        AudioManager.instance.Play("Menu1");
        Debug.Log("Campaign Button Pressed");
        SceneManager.LoadScene("SkirmishSetup");
    }

    public void Skirmish()
    {
        AudioManager.instance.Play("Menu1");
        Debug.Log("Skirmish Button Pressed");
        SceneManager.LoadScene("SkirmishSetup");
    }

    public void HowToPlayClicked()
    {
        AudioManager.instance.Play("Menu1");
        Debug.Log("How To Play Button Pressed");
        SceneManager.LoadScene("HowToPlay");
    }

    public void SettingsOpen()
    {
        AudioManager.instance.Play("Menu1");
        Debug.Log("Settings Button Pressed");
        MainMenuAnimator.SetBool("Open", false);
        SettingsAnimator.SetBool("Open", true);
    }

    public void SettingsClose()
    {
        AudioManager.instance.Play("Menu2");
        Debug.Log("Settings Back Button Pressed");
        MainMenuAnimator.SetBool("Open", true);
        SettingsAnimator.SetBool("Open", false);
    }

    public void Quit()
    {
        AudioManager.instance.Play("Menu2");
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }
}
