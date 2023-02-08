using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSetupController : MonoBehaviour
{

    public static GameSetupController instance;

    private IDEController IDE;                  // Reference to the IDE used for editing scripts
    private ControlPanelManager ControlPanel;   // Reference to the Control Panel used for creating/ editing/ removing/ adding scripts

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        // Get a reference to the IDE and clear all text
        IDE = IDEController.instance;
        IDE.Clear();
        // Get a reference to the Control Panel and make it load all buttons on startup
        ControlPanel = ControlPanelManager.instance;
        ControlPanel.Load();
    }


    /**
     * Creates a script, Hides the control panel and opens the IDE with a blank script
     **/ 
    public void CreateScriptStart()
    {
        ControlPanel.Hide();
        IDE.Clear();
        IDE.Open();
    }

    /**
     * Similar to creating, this will close the control panel and open the IDE, but with a script loaded using the filename 
     **/
    public void EditScriptStart(string filename, int scriptIndex)
    {
        ControlPanel.Hide();
        IDE.Clear();
        IDE.Open(filename, scriptIndex);
    }

    public void RemoveScript(int index)
    {
        ControlPanel.Delete(index);
        ControlPanel.Load();
    }

    public void DeleteScript(string filename)
    {
        FileManager.DeleteFile(filename);
        ControlPanel.DeleteAll(filename);
        ControlPanel.Load();
    }

    public void LoadScript(string filename)
    {
        ControlPanel.Add(filename);
        ControlPanel.Load();
    }
    
    public void CreateScriptComplete(string filename)
    {
        IDE.Close();
        ControlPanel.Show();
        ControlPanel.Add(filename);
        ControlPanel.Load();
    }

    public void EditScriptComplete(string filename, int scriptIndex)
    {
        IDE.Close();
        ControlPanel.Show();
        ControlPanel.EditComplete(filename, scriptIndex);
        ControlPanel.Load();
    }


}
