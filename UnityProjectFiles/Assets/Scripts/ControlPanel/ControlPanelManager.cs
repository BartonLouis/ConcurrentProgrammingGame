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

    private GameSetupController controller;
    private List<string> scripts = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        scripts = new List<string>();
        controller = GameSetupController.instance;
    }
    
    public void Load()
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

    public void New()
    {
        controller.CreateScriptStart();
    }

    public void Edit(string filename, int scriptIndex)
    {
        controller.EditScriptStart(filename, scriptIndex);
    }

    public void Remove(int index)
    {
        controller.RemoveScript(index);
    }

    public void Delete(int index)
    {
        scripts.RemoveAt(index);
    }

    public void DeleteAll(string name)
    {
        scripts.RemoveAll((s) => { return s == name; });
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

    public void EditComplete(string filename, int index)
    {
        scripts[index] = filename;
    }

    public void Add(string filename)
    {
        scripts.Add(filename);
    }

    public void Load(string filename)
    {
        controller.LoadScript(filename);
    }

}
