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
    private bool clicked = false;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        anim = GetComponent<Animator>();
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
        clicked = false;
        anim.SetBool("Open", true);
    }

    public void Open(string name)
    {
        clicked = false;
        string text = FileManager.LoadFile(name);
        code.text = text;
        scriptName.text = name;
        debugConsole.text = name + "\n" + text;
        anim.SetBool("Open", true);
    }

    public void ValueChanged()
    {
        clicked = false;
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
        yield return new WaitForSeconds(1);
        // Check that script has been given name
        if (scriptName.text == "")
        {
            debugConsole.text = "No Name Provided!";
            anim.SetBool("Submitting", false);
            anim.SetTrigger("Error");
        }
        // Check that some code has been submitted
        else if (code.text == "")
        {
            debugConsole.text = "No Code Provided!";
            anim.SetBool("Submitting", false);
            anim.SetTrigger("Error");
        } else if (FileManager.GetFileNames().Contains(scriptName.text) && !clicked)
        {
            clicked = true;
            debugConsole.text = "A script with that name already exists\n\t press Submit again to overwrite file...";
            anim.SetBool("Submitting", false);
            anim.SetTrigger("Error");
        } else
        {
            string expression = code.text;
            string filename = scriptName.text;
            debugConsole.text = "Success!";

            // parse and save file
            bool parseSuccessful = true;
            if (parseSuccessful)
            {
                FileManager.SaveFile(filename, expression);
                Close();
                ControlPanelManager.instance.Show();
                ControlPanelManager.instance.Add(filename);
            } else
            {
                debugConsole.text = "Parse Unsuccessful!";
                anim.SetBool("Submitting", false);
                anim.SetTrigger("Error");
            }
        }
    }

}
