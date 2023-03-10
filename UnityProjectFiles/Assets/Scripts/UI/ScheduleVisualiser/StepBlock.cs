using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpreter;

public class StepBlock : MonoBehaviour
{

    public Sprite DamageIcon;
    public Sprite SupportIcon;
    public Sprite TankIcon;
    public Color EnemyColour;
    public Color TeammateColour;
    private Transform Line;

    public Image Back;
    public Image Icon;

    private bool active = false;
    public void Start()
    {
        Line = GameObject.Find("CurrentStepLine").transform;
    }

    public void SetCharacter(Character character)
    {
        if (character != null)
        {
            if (character.Team.TeamNum == 1)
            {
                Back.color = TeammateColour;
            }
            else
            {
                Back.color = EnemyColour;
            }

            if (character.ClassType == ClassValue.ClassType.Damage)
            {
                Icon.sprite = DamageIcon;
            }
            else if (character.ClassType == ClassValue.ClassType.Support)
            {
                Icon.sprite = SupportIcon;
            }
            else
            {
                Icon.sprite = TankIcon;
            }
        } else
        {
            Back.color = Color.clear;
            Icon.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (active) return;
        if (Line.position.x > transform.position.x)
        {
            active = true;
            GetComponent<Animator>().SetTrigger("Active");
        }
        //Vector3 targetDir = transform.position - Line.position;
        //Vector3 perp = Vector3.Cross(transform.forward, targetDir);
        //float dir = Vector3.Dot(perp, transform.up);
        //if (dir < 0)
        //{
        //    Debug.Log("Activating");
        //    GetComponent<Animator>().SetTrigger("Active");
        //    active = true;
        //}
    }
}
