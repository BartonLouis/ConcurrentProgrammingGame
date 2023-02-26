using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ControlPanelAddButton : MonoBehaviour
{
   
    [SerializeField] Animator animator;
    [SerializeField] GameObject AddButton;
    [SerializeField] GameObject ChooseOption;
    [SerializeField] Transform Parent;
    [SerializeField] GameObject SubButtonPrefab;
    [SerializeField] GameObject FinalButtonPrefab;
    private ControlPanelManager manager;


    private void Start()
    {
        animator.SetBool("Open", true);
        manager = ControlPanelManager.instance;
        AddButton.SetActive(true);
        ChooseOption.SetActive(false);
    }
    public void OnClicked()
    {
        AddButton.SetActive(false);
        ChooseOption.SetActive(true);
        LoadButtons();
    }

    private void LoadButtons()
    {
        foreach(Transform child in Parent)
        {
            Destroy(child.gameObject);
        }
        foreach(string filename in FileManager.GetFileNames())
        {
            GameObject btn = Instantiate(SubButtonPrefab, Parent);
            btn.GetComponent<ChooseScript>().manager = this;
            btn.GetComponent<ChooseScript>().fileName = filename;
        }
        GameObject lastBtn = Instantiate(FinalButtonPrefab, Parent);
        lastBtn.GetComponent<NewScriptButton>().manager = this;
    }

    public void LoadScript(string filename)
    {
        manager.Load(filename);
    }

    public void NewScript()
    {
        manager.New();
    }

    public void DeleteScript(string scriptName)
    {
        manager.DeleteScript(scriptName);
    }

    public void Hide()
    {
        foreach (Transform child in Parent)
        {
            child.GetComponent<Animator>().SetBool("Open", false);
        }
    }

    public void Show()
    {
        foreach (Transform child in Parent)
        {
            child.GetComponent<Animator>().SetBool("Open", true);
        }
    }
}
