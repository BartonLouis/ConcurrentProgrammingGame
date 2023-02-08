using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Interpreter;

public class IDEController : MonoBehaviour
{
    public enum IDEMode
    {
        EDIT,
        CREATE
    };

    public static IDEController instance;

    public TMP_InputField code;
    public TMP_InputField scriptName;
    public TextMeshProUGUI debugConsole;

    private GameSetupController controller;
    private Animator anim;
    private bool clicked = false;
    private int scriptIndex = -1;
    private IDEMode mode;
    

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GameSetupController.instance;
    }

    public void Clear()
    {
        code.text = "";
        scriptName.text = "";
        debugConsole.text = "";
        clicked = false;
    }

    public void Open()
    {
        Clear();
        anim.SetBool("Open", true);
        mode = IDEMode.CREATE;
    }

    public void Open(string name, int index)
    {
        scriptIndex = index;
        code.text = FileManager.LoadFile(name);
        scriptName.text = name;
        clicked = false;
        anim.SetBool("Open", true);
        mode = IDEMode.EDIT;
    }

    public void ValueChanged()
    {
        clicked = false;
    }
    

    public void SubmitClicked()
    {
        anim.SetBool("Submitting", true);
        StartCoroutine(Submit());
    }

    public void Close()
    {
        anim.SetBool("Submitting", false);
        anim.SetBool("Open", false);
    }

    IEnumerator Submit()
    {
        yield return new WaitForSeconds(1);
        string expression = code.text;
        string filename = scriptName.text;
        bool parseSuccessful = true;
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
        // If filename already exists, give the option to overwrite or change filename
        } else if (FileManager.GetFileNames().Contains(scriptName.text) && !clicked)
        {
            clicked = true;
            debugConsole.text = "A script with that name already exists\n\t press Submit again to overwrite file...";
            anim.SetBool("Submitting", false);
            anim.SetTrigger("Error");
        // Check that parse is successful
        } else if (parseSuccessful)
        {
            CompleteSubmit(filename, expression);
        // Give error message
        } else
        {
            debugConsole.text = "Parse Unsuccessful!";
            anim.SetBool("Submitting", false);
            anim.SetTrigger("Error");
        }
    }

    private void CompleteSubmit(string filename, string expression)
    {
        FileManager.SaveFile(filename, expression);
        ControlPanelManager.instance.Show();
        if (mode == IDEMode.CREATE)
        {
            controller.CreateScriptComplete(filename);
        } else
        {
            controller.EditScriptComplete(filename, scriptIndex);
        }
    }

}
