using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopicButton : MonoBehaviour
{

    
    [HideInInspector] public string Name;
    [HideInInspector] public int Index;


    public void Setup(int index, string name)
    {
        Index = index;
        Name = name;
        GetComponentInChildren<TextMeshProUGUI>().text = name;
    }

    public void Clicked()
    {
        AudioManager.instance.Play("Menu1");
        DocumentationController.instance.ButtonClicked(Index);
    }





}
