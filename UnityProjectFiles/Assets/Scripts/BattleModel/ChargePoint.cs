using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePoint : MonoBehaviour
{
    public TeamCenter Team;
    private LineRenderer Line;
    public bool Locked;
    public Character Target;

    private void Start()
    {
        Line = GetComponent<LineRenderer>();
        Line.enabled = false;
        Locked = false;
    }

    public void OnBattleBegin()
    {
        Line.enabled = false;
        Target = null;
        Locked = false;
    }

    public void Lock(Character target)
    {
        Debug.Log("Getting Locked By " + target);
        if (!Locked)
        {
            Target = target;
            Line.enabled = true;
            Line.positionCount = 2;
            Line.SetPosition(1, transform.position);
            Line.SetPosition(0, target.transform.position);
        }
    }

    public void Unlock()
    {
        Locked = false;
        Target = null;
        Line.enabled = false;
    }

    public void OnBattleEnd()
    {
        Line.enabled = false;
        Locked = false;
    }
}
