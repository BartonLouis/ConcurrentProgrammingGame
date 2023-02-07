using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubButton : MonoBehaviour
{
    [HideInInspector] public bool isForNewScript;
    [HideInInspector] public string fileName;
    [HideInInspector] public ControlPanelAddButton manager;

    [SerializeField] TextMeshProUGUI fileNameText;

    private void Start()
    {
        if (!isForNewScript)
        {
            fileNameText.text = fileName;
        } else
        {
            fileNameText.text = "New Script+";
        }
    }

    public void OnClicked()
    {
        if (isForNewScript)
        {
            manager.NewScript();
        } else
        {
            manager.LoadScript(fileName);
        }
    }
}
