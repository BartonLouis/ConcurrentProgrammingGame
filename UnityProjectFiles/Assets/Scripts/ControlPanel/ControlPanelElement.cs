using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlPanelElement : MonoBehaviour
{
    [HideInInspector] public int index = 0;
    [HideInInspector] public string scriptName;
    private ControlPanelManager manager;

    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] Animator animator;
    private void Start()
    {
        NameText.text = scriptName;
        animator.SetBool("Open", true);
        manager = ControlPanelManager.instance;
    }

    public void EditButtonClicked()
    {
        manager.Edit(index);
    }

    public void DeleteButtonClicked()
    {
        manager.Delete(index);
    }
}
