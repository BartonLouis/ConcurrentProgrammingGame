using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Interpreter;

public class IDEController : MonoBehaviour
{

    private static IDEController instance;

    public TMP_InputField code;
    public TMP_InputField scriptName;
    public TextMeshProUGUI debugConsole;

    private Animator anim;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
        anim.SetBool("Submitting", false);
        debugConsole.text = "";
    }

    public static void OpenIDE()
    {
        instance.Open();
    }

    public static void OpenIDE(string name)
    {
        instance.Open(name);
    }

    public void Open()
    {
        code.text = "";
        scriptName.text = "";
        debugConsole.text = "";
        anim.SetBool("Open", true);
    }

    public void Open(string name)
    {
        // Load file
        // TODO
        code.text = "";
        scriptName.text = name;
        debugConsole.text = "";
        anim.SetBool("Open", true);
        Open();
    }

    public void Submit()
    {
        anim.SetBool("Submitting", true);
        StartCoroutine(CompleteSubmit());
    }
    
    public void Close()
    {
        anim.SetBool("Submitting", false);
        anim.SetBool("Open", false);
    }

    IEnumerator CompleteSubmit()
    {
        Debug.Log(code.text);
        Debug.Log(scriptName.text);
        yield return new WaitForSeconds(1);
        
        if (scriptName.text == "")
        {
            debugConsole.text = "No Name Provided!";
            anim.SetBool("Submitting", false);
            anim.SetTrigger("Error");
        }
        else if (code.text == "")
        {
            debugConsole.text = "No Code Provided!";
            anim.SetBool("Submitting", false);
            anim.SetTrigger("Error");
        } else
        {
            string expression = code.text;
            string filename = scriptName.text;
            // parse and save file
            // TODO
            Close();
            ControlPanelManager.instance.Show();
            ControlPanelManager.instance.Add(filename);
        }
    }

}
