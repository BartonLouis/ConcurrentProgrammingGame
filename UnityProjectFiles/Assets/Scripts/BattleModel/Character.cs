using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public abstract class Character : MonoBehaviour
{
    [SerializeField] GameObject EnergyBarPrefab;
    
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
    private EnergyBar EnergyBar;

    private List<KeyValuePair<float, int>> DamageMultipliers;
    private List<KeyValuePair<float, int>> DefenseMultipliers;

    private float currentHealth;
    private bool alive = true;
    private bool charged = false;
    

    public void Setup()
    {
        GameObject worldCanvas = GameObject.Find("WorldCanvas");
        GameObject energyBar = Instantiate(EnergyBarPrefab, worldCanvas.transform);
        energyBar.transform.position = transform.position;
        EnergyBar = energyBar.GetComponent<EnergyBar>();
        string sourceCode = FileManager.LoadFile(ScriptFilename);
        RuntimeInstance = new RuntimeInstance(sourceCode);
        RuntimeInstance.BindEnergyBar(EnergyBar);
        RuntimeInstance.Character = this;

        DamageMultipliers = new List<KeyValuePair<float, int>>();
        DefenseMultipliers = new List<KeyValuePair<float, int>>();
        currentHealth = BaseMaxHealth;
        charged = false;
        alive = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(10);
        }
    }

    public void Step()
    {
        RuntimeInstance.Step();
        List<KeyValuePair<float, int>> toRemove = new List<KeyValuePair<float, int>>();
        // Decrement all multipliers and remove them if they have 0 time steps left
        for(int i = 0; i < DamageMultipliers.Count; i++)
        {
            DamageMultipliers[i] = new KeyValuePair<float, int>(DamageMultipliers[i].Key, DamageMultipliers[i].Value - 1);
            if (DamageMultipliers[i].Value <= 0) toRemove.Add(DamageMultipliers[i]);
        }
        foreach(KeyValuePair<float, int> multipler in toRemove)
        {
            DamageMultipliers.Remove(multipler);
        }
        toRemove.Clear();
        // Decrement all defense multipliers and remove them if they have 0 time steps left
        for (int i = 0; i < DefenseMultipliers.Count; i++)
        {
            DefenseMultipliers[i] = new KeyValuePair<float, int>(DefenseMultipliers[i].Key, DefenseMultipliers[i].Value - 1);
            if (DefenseMultipliers[i].Value <= 0) toRemove.Add(DefenseMultipliers[i]);
        }
        foreach(KeyValuePair<float, int> multiplier in toRemove)
        {
            DefenseMultipliers.Remove(multiplier);
        }
    }

    public void AddDefenseMultiplier(float multiplier, int time)
    {
        DefenseMultipliers.Add(new KeyValuePair<float, int>(multiplier, time));
    }

    public void AddDamageMultipler(float multiplier, int time)
    {
        DamageMultipliers.Add(new KeyValuePair<float, int>(multiplier, time));
    }

    public void Damage(float amount)
    {
        float totalMultiplier = 1;
        foreach(KeyValuePair<float, int> multipler in DefenseMultipliers)
        {
            totalMultiplier += multipler.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        // Subtract adjusted amount from health and cap it at 0
        amount *= totalMultiplier;
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        Debug.Log(this.ToString() + " Taking " + amount + " damage. Health remaining:" + currentHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, BaseMaxHealth);
    }

    public void Die()
    {
        Debug.Log(this + " Dieing");
        alive = false;
    }

    public bool IsAlive()
    {
        return alive;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return BaseMaxHealth;
    }

    public bool IsMaxHealth()
    {
        return (currentHealth == BaseMaxHealth);
    }

    public bool IsCharged()
    {
        return charged;
    }

    public void EndOfStepUpdate()
    {
        // Every character runs this function at the end of each turn. 
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Attack(Value target)
    {
        float damage = BaseDamage;
        float totalMultiplier = 1;
        foreach(KeyValuePair<float, int> multipler in DamageMultipliers)
        {
            totalMultiplier += multipler.Value;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        damage *= totalMultiplier;
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

    public virtual void Heal(Value target)
    {
        Debug.Log(this + "Healing Teammate");
    }

    public virtual void Boost(Value target)
    {
        Debug.Log(this + "Boosting Teammate");
    }

    public virtual void Defend(Value target)
    {
        Debug.Log(this + "Defending Teammate");
    }

    public virtual void Block(Value target)
    {
        Debug.Log(this + "Blocking Enemy");
    }

    public virtual void Lock(Value side)
    {
        Debug.Log(this + "Locking");
    }

    public virtual void ChargeUp()
    {
        Debug.Log(this + "Charging Up");
    }

    public virtual void SendMessageTo(Value player, Value message)
    {
        Debug.Log(this + "Sending Message");
    }

    public virtual void SendMessageToAll(Value message)
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

    public void ReceiveMessage(MessageValue message)
    {
        RuntimeInstance.ReceiveMessage(message);
    }

    public override string ToString()
    {
        return "" + Team.TeamNum + CharacterNum;
    }

    public void OnGameEnd()
    {
        Destroy(EnergyBar.gameObject);
    }
}
