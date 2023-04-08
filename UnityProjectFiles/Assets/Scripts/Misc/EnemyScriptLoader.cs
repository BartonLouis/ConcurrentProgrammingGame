using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyScriptLoader : MonoBehaviour
{
    [System.Serializable]
    public class Script
    {
        public string name;
        public TextAsset script;
    }

    [SerializeField]
    public Script[] scripts;
    

    public string LoadScript(string scriptName)
    {
        Debug.Log("Trying to load: " + scriptName);
        Script scriptText = Array.Find(scripts, script => script.name == scriptName);
        if (scriptText == null)
        {
            Debug.Log("Tried to load Script: " + scriptName + " but it didn't exist");
            return LoadScript("Default");
        } else
        {
            Debug.Log("Found: " + scriptText.name);
            return scriptText.script.text;
        }


    }
}
