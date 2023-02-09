using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseScript : MonoBehaviour
{
    [HideInInspector] public string fileName;
    [HideInInspector] public ControlPanelAddButton manager;

    [SerializeField] TextMeshProUGUI fileNameText;

    private void Start()
    {
        fileNameText.text = fileName;
        GetComponent<Animator>().SetBool("Open", true);
    }


    public void OnSelectClicked()
    {
        manager.LoadScript(fileName);
    }

    public void OnDeleteClicked()
    {
        manager.DeleteScript(fileName);
    }
}
