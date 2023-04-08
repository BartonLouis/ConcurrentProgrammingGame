using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    private Animator MainMenuAnimator;
    private Animator SettingsAnimator;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider EffectsSlider;

    private void Start()
    {
        MainMenuAnimator = GameObject.Find("Main Menu").GetComponent<Animator>();
        SettingsAnimator = GameObject.Find("Settings Menu").GetComponent<Animator>();
        AudioManager.instance.PlayMusic("MainMenu");
        MasterSlider.value = AudioManager.instance.GetVolume("MasterVolume");
        MusicSlider.value = AudioManager.instance.GetVolume("MusicVolume");
        EffectsSlider.value = AudioManager.instance.GetVolume("EffectsVolume");
    }

    public void Campaign()
    {
        AudioManager.instance.Play("Menu1");
        Debug.Log("Campaign Button Pressed");
        SceneManager.LoadScene("CampaignMap");
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


    // Settings bits

    public void MasterVolumeChanged(float volume)
    {
        Debug.Log(volume);
        AudioManager.instance.ChangeVolume("MasterVolume", volume);
    }

    public void MusicVolumeChanged(float volume)
    {
        Debug.Log(volume);
        AudioManager.instance.ChangeVolume("MusicVolume", volume);
    }

    public void EffectsVolumeChanged(float volume)
    {
        Debug.Log(volume);
        AudioManager.instance.ChangeVolume("EffectsVolume", volume);
    }
}
