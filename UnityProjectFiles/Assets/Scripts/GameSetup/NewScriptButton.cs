using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewScriptButton : MonoBehaviour
{
    [HideInInspector] public string fileName;
    [HideInInspector] public ControlPanelAddButton manager;

    [SerializeField] TextMeshProUGUI fileNameText;
    public void Start()
    {
        GetComponent<Animator>().SetBool("Open", true);
    }

    public void OnClicked()
    {
        manager.NewScript();
    }

}
