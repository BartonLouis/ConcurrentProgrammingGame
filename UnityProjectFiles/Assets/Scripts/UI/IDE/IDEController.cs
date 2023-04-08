using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    private GameController Controller;
    private Animator anim;
    private bool clicked = false;
    private bool nameChanged = false;
    private int scriptIndex = -1;
    private IDEMode mode;
    

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        Controller = GameController.instance;
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
        nameChanged = false;
    }

    public void Open(string name, int index)
    {
        scriptIndex = index;
        code.text = FileManager.LoadFile(name);
        scriptName.text = name;
        clicked = false;
        anim.SetBool("Open", true);
        mode = IDEMode.EDIT;
        nameChanged = false;
    }

    public void CodeChanged()
    {
        clicked = false;
    }
    
    public void NameChanged()
    {
        nameChanged = true;
    }

    public void SubmitClicked()
    {
        string expression = code.text;
        string filename = scriptName.text;
        RuntimeInstance runtimeInstance = new RuntimeInstance(expression);
        bool parseSuccessful = runtimeInstance.GetParseResult();
        if (scriptName.text == "")
        {
            AudioManager.instance.Play("Menu3");
            debugConsole.text = "No Name Provided!";
            anim.SetTrigger("Error");
        }
        // Check that some code has been submitted
        else if (code.text == "")
        {
            AudioManager.instance.Play("Menu3");
            debugConsole.text = "No Code Provided!";
            anim.SetTrigger("Error");
            // If filename already exists, give the option to overwrite or change filename
        }
        else if (FileManager.GetFileNames().Contains(scriptName.text) && !clicked && (mode != IDEMode.EDIT || (mode == IDEMode.EDIT && nameChanged)))
        {
            AudioManager.instance.Play("Menu3");
            nameChanged = false;
            clicked = true;
            debugConsole.text = "A script with that name already exists\n\t press Submit again to overwrite file...";
            anim.SetTrigger("Error");
        }
        else if (parseSuccessful)
        {
            CompleteSubmit(filename, expression);
            AudioManager.instance.Play("Menu1");
        }
        else
        {
            AudioManager.instance.Play("Menu3");
            debugConsole.text = "Parse Unsuccessful!";
            foreach (string error in runtimeInstance.GetErrors())
            {
                debugConsole.text += $"\n\t {error}";
            }
            anim.SetTrigger("Error");
        }
    }

    public void ExitClicked()
    {
        AudioManager.instance.Play("Menu2");
        if (!clicked)
        {
            clicked = true;
            debugConsole.text = "Any unsaved changes will be lost..." +
                "\nAre you sure you want to exit without saving?";
        } else
        {
            Controller.CancelScript();
        }
    }

    public void Close()
    {
        anim.SetBool("Open", false);
    }

    private void CompleteSubmit(string filename, string expression)
    {
        FileManager.SaveFile(filename, expression);
        ControlPanelManager.instance.Show();
        if (mode == IDEMode.CREATE)
        {
            Controller.CreateScriptComplete(filename);
        } else
        {
            Controller.EditScriptComplete(filename, scriptIndex);
        }
    }

}
