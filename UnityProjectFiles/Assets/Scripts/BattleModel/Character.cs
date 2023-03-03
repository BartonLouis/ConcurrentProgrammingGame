using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float BaseMaxHealth  = 100;
    [SerializeField] protected float BaseDamage     = 20;
    [SerializeField] protected float BaseSelfHeal   = 10;
    [SerializeField] protected float BaseTeamHeal   = 0;
    [SerializeField] protected float BaseSelfDefend = 0.3f;
    [SerializeField] protected float BaseTeamDefend = 0.0f;
    [SerializeField] protected float BaseBoost      = 0.0f;
    [SerializeField] protected float BaseBlock      = 0.0f;

    [HideInInspector] public ClassValue.ClassType ClassType;
    [HideInInspector] public TeamCenter Team;
    [HideInInspector] public int CharacterNum;

    [HideInInspector] public string ScriptFilename;
    private RuntimeInstance RuntimeInstance;

    public void Setup()
    {
        string sourceCode = FileManager.LoadFile(ScriptFilename);
        RuntimeInstance = new RuntimeInstance(sourceCode);
        RuntimeInstance.Character = this;
    }

    public void Step()
    {
        Debug.Log(this + " Stepping");
        RuntimeInstance.Step();
    }

    public virtual void Attack()
    {
        Debug.Log(this + "Attacking");
    }

    public virtual void HealSelf()
    {
        Debug.Log(this + "Healing Self");
    }

    public virtual void DefendSelf()
    {
        Debug.Log(this + "Defending Self");
    }

    public virtual void Heal()
    {
        Debug.Log(this + "Healing Teammate");
    }

    public virtual void Boost()
    {
        Debug.Log(this + "Boosting Teammate");
    }

    public virtual void Defend()
    {
        Debug.Log(this + "Defending Teammate");
    }

    public virtual void Block()
    {
        Debug.Log(this + "Blocking Enemy");
    }

    public virtual void Lock()
    {
        Debug.Log(this + "Locking");
    }

    public virtual void ChargeUp()
    {
        Debug.Log(this + "Charging Up");
    }

    public virtual void SendMessageTo()
    {
        Debug.Log(this + "Sending Message");
    }

    public virtual void SendMessageToAll()
    {
        Debug.Log("Sending Message To All");
    }

    public virtual void Listen()
    {
        Debug.Log("Listening");
    }

    public virtual void Yield()
    {
        Debug.Log("Yielding");
    }

    public override string ToString()
    {
        return "Team: " + Team.TeamNum + " Character: " + CharacterNum + "> ";
    }

}
