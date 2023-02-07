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
        Debug.Log("Loading Button");
        foreach(Transform child in Parent)
        {
            Destroy(child.gameObject);
        }
        foreach(string filename in FileManager.GetFileNames())
        {
            GameObject btn = Instantiate(SubButtonPrefab, Parent);
            btn.GetComponent<SubButton>().isForNewScript = false;
            btn.GetComponent<SubButton>().manager = this;
            btn.GetComponent<SubButton>().fileName = filename;
        }
        GameObject lastBtn = Instantiate(SubButtonPrefab, Parent);
        lastBtn.GetComponent<SubButton>().isForNewScript = true;
        lastBtn.GetComponent<SubButton>().manager = this;
    }

    public void LoadScript(string filename)
    {
        Debug.Log("Loading: " + filename);
        manager.Add(filename);
    }

    public void NewScript()
    {
        Debug.Log("Creating new Script!");
        manager.Hide();
        IDEController.OpenIDE();
    }
}
