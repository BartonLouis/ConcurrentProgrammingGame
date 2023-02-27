using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class ControlPanelManager : MonoBehaviour
{

    public static ControlPanelManager instance;
    [SerializeField] GameObject ControlElementPrefab;
    [SerializeField] GameObject ControlPanelAddButtonPrefab;
    [SerializeField] Transform Parent;
    [SerializeField] Animator animator;

    private GameController Controller;
    private List<Tuple<string, ClassValue.ClassType>> scripts = new List<Tuple<string, ClassValue.ClassType>>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        scripts = new List<Tuple<string, ClassValue.ClassType>>();
        Controller = GameController.instance;
    }
    
    public void Load(int maxPlayers)
    {
        foreach(Transform child in Parent)
        {
            Destroy(child.gameObject);
        }
        int index = 0;
        foreach (Tuple<string, ClassValue.ClassType> script in scripts)
        {
            GameObject btnObj = Instantiate(ControlElementPrefab, Parent);
            btnObj.GetComponent<ControlPanelElement>().index = index;
            btnObj.GetComponent<ControlPanelElement>().scriptName = script.Item1;
            btnObj.GetComponent<ControlPanelElement>().ClassType = script.Item2;
            index++;
        }
        if (scripts.Count < maxPlayers)
        {
            GameObject lastButton = Instantiate(ControlPanelAddButtonPrefab, Parent);
        }
    }

    public void New()
    {
        Controller.CreateScriptStart();
    }

    public void Edit(string filename, int scriptIndex)
    {
        Controller.EditScriptStart(filename, scriptIndex);
    }

    public void Remove(int index)
    {
        Controller.RemoveScript(index);
    }

    public void Delete(int index)
    {
        scripts.RemoveAt(index);
        Controller.RemovePlayer(index);
    }

    public void DeleteAll(string name)
    {
        scripts.RemoveAll((s) => {
            if (s.Item1 == name)
            {
                Controller.RemovePlayer(scripts.IndexOf(s));
                return true;
            } else
            {
                return false;
            }
        });
    }

    public void DeleteScript(string name)
    {
        Controller.DeleteScript(name);
    }

    public void Hide()
    {
        foreach(Transform child in Parent)
        {
            child.GetComponent<Animator>().SetBool("Open", false);
            try
            {
                child.GetComponent<ControlPanelAddButton>().Hide();
            }
            catch (Exception) { }
        }
        animator.SetBool("Open", false);
    }

    public void Show()
    {
        foreach (Transform child in Parent)
        {
            child.GetComponent<Animator>().SetBool("Open", true);
            try
            {
                child.GetComponent<ControlPanelAddButton>().Show();
            }
            catch (Exception) { }
        }
        animator.SetBool("Open", true);
    }

    public void EditComplete(string filename, int index)
    {
        scripts[index]= new Tuple<string, ClassValue.ClassType>(filename, scripts[index].Item2);
    }

    public void Add(string filename)
    {
        scripts.Add(new Tuple<string, ClassValue.ClassType>(filename, ClassValue.ClassType.Damage));
        Controller.AddPlayer(ClassValue.ClassType.Damage);
    }

    public void Load(string filename)
    {
        Controller.LoadScript(filename);
    }

    public void UpdateClass(int index, ClassValue.ClassType classType)
    {
        scripts[index] = new Tuple<string, ClassValue.ClassType>(scripts[index].Item1, classType);
        Controller.UpdatePlayerClass(index, classType);
    }

}
