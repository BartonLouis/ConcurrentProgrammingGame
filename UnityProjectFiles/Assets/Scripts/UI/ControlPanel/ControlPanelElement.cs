using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Interpreter;

public class ControlPanelElement : MonoBehaviour
{
    [HideInInspector] public int index = 0;
    [HideInInspector] public string scriptName;
    [HideInInspector] public ClassValue.ClassType ClassType = ClassValue.ClassType.Damage;

    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] Image CharacterIcon;
    [SerializeField] Animator animator;
    [SerializeField] Sprite DamageIcon;
    [SerializeField] Sprite SupportIcon;
    [SerializeField] Sprite TankIcon;

    private ControlPanelManager manager;
    

    private void Start()
    {
        NameText.text = scriptName;
        animator.SetBool("Open", true);
        manager = ControlPanelManager.instance;
        switch (ClassType)
        {
            case ClassValue.ClassType.Damage:
                CharacterIcon.sprite = DamageIcon;
                break;
            case ClassValue.ClassType.Support:
                CharacterIcon.sprite = SupportIcon;
                break;
            case ClassValue.ClassType.Tank:
                CharacterIcon.sprite = TankIcon;
                break;

        }
    }

    public void EditButtonClicked()
    {
        AudioManager.instance.Play("Menu3");
        manager.Edit(scriptName, index);
    }

    public void DeleteButtonClicked()
    {
        AudioManager.instance.Play("Menu2");
        manager.Remove(index);
    }

    public void IconClicked()
    {
        AudioManager.instance.Play("Menu3");
        switch (ClassType)
        {
            case ClassValue.ClassType.Damage:
                ClassType = ClassValue.ClassType.Support;
                CharacterIcon.sprite = SupportIcon;
                break;
            case ClassValue.ClassType.Support:
                ClassType = ClassValue.ClassType.Tank;
                CharacterIcon.sprite = TankIcon;
                break;
            case ClassValue.ClassType.Tank:
                ClassType = ClassValue.ClassType.Damage;
                CharacterIcon.sprite = DamageIcon;
                break;
        }
        manager.UpdateClass(index, ClassType);
    }
}
