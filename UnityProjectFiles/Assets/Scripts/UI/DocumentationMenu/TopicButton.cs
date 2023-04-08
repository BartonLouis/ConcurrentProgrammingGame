using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopicButton : MonoBehaviour
{

    public int normalHeight = 40;
    public int subHeight = 25;
    [HideInInspector] public string Name;
    [HideInInspector] public int Index;


    public void Setup(int index, string name, bool subCategory)
    {
        Index = index;
        Name = name;
        RectTransform rt = GetComponent<RectTransform>();
        if (subCategory)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, subHeight);
            GetComponentInChildren<TextMeshProUGUI>().text = " - " + name;
        }
        else
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, normalHeight);
            GetComponentInChildren<TextMeshProUGUI>().text = name;
        }
    }

    public void Clicked()
    {
        AudioManager.instance.Play("Menu1");
        DocumentationController.instance.ButtonClicked(Index);
    }





}
