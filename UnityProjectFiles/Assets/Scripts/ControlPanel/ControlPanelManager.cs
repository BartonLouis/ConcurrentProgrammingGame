using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelManager : MonoBehaviour
{

    public static ControlPanelManager instance;
    [SerializeField] GameObject ControlElementPrefab;
    [SerializeField] GameObject ControlPanelAddButtonPrefab;
    [SerializeField] Transform Parent;
    [SerializeField] Animator animator;

    private List<string> scripts = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        scripts = new List<string>();
        Load();
    }
    
    private void Load()
    {
        foreach(Transform child in Parent)
        {
            Destroy(child.gameObject);
        }
        int index = 0;
        foreach (string name in scripts)
        {
            GameObject btnObj = Instantiate(ControlElementPrefab, Parent);
            btnObj.GetComponent<ControlPanelElement>().index = index;
            btnObj.GetComponent<ControlPanelElement>().scriptName = name;
            index++;
        }
        GameObject lastButton = Instantiate(ControlPanelAddButtonPrefab, Parent);
    }

    public void Edit(string filename)
    {
        Hide();
        IDEController.OpenIDE(filename);
    }

    public void Delete(int index)
    {
        Debug.Log("Deleting Script Number: " + index);
        scripts.RemoveAt(index);
        Load();
    }

    public void Hide()
    {
        foreach(Transform child in Parent)
        {
            child.GetComponent<Animator>().SetBool("Open", false);
        }
        animator.SetBool("Open", false);
    }

    public void Show()
    {
        foreach (Transform child in Parent)
        {
            child.GetComponent<Animator>().SetBool("Open", true);
        }
        animator.SetBool("Open", true);
    }

    public void Add(string filename)
    {
        Debug.Log("Adding a new Script: " + filename);
        // Add script and reload
        scripts.Add(filename);
        Load();
    }

}
