using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelAddButton : MonoBehaviour
{
   
    [SerializeField] Animator animator;
    private ControlPanelManager manager;
    private void Start()
    {
        animator.SetBool("Open", true);
        manager = ControlPanelManager.instance;
    }
    public void OnClicked()
    {
        Debug.Log("Clicked!");
        manager.Hide();
        IDEController.OpenIDE();
    }
}
