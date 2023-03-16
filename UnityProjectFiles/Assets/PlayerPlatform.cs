using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatform : MonoBehaviour
{
    private Animator Animator;
    public Color TeamColor = new Color(0, 1, 0);
    public Color EnemyColor = new Color(1, 0, 0);

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void SetTeam(TeamCenter team)
    {
        if (team.TeamNum == 1)
        {

            GetComponent<SpriteRenderer>().color = TeamColor;
        } else
        {
            GetComponent<SpriteRenderer>().color = EnemyColor;
        }
    }

    public void Hide()
    {
        Animator.SetBool("Visible", false);
    }

    public void Show()
    {
        Animator.SetBool("Visible", true);
    }
}
