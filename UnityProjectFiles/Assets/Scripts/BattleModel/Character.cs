using Interpreter;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected int BuffTime = 20;

    [Space(10)]

    [SerializeField] GameObject EnergyBarPrefab;
    [SerializeField] GameObject HealthBarPrefab;
    [SerializeField] GameObject PlatformPrefab;
    [SerializeField] GameObject DamageTextPrefab;
    [SerializeField] GameObject DefenseBuffStackPrefab;
    [SerializeField] GameObject BuffStackPrefab;
    [SerializeField] GameObject DebuffStackPrefab;

    [Space(10)]
    
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


    protected Animator Anim;
    protected RuntimeInstance RuntimeInstance;

    private EnergyBar EnergyBar;
    private HealthBar HealthBar;
    private PlayerPlatform Platform;
    private BuffStack DefenseStack;
    private BuffStack BuffStack;
    private BuffStack DebuffStack;

    protected List<KeyValuePair<float, int>> DamageMultipliers;
    protected List<KeyValuePair<float, int>> DefenseMultipliers;

    private float currentHealth;
    private bool alive = true;
    private bool charged = false;
    private bool tookTurn = false;

    private BattleModel BattleModel;
    

    public void Setup()
    {
        // Visual Elements
        GameObject worldCanvas = GameObject.Find("WorldCanvas");
        GameObject energyBar = Instantiate(EnergyBarPrefab, worldCanvas.transform);
        GameObject healthBar = Instantiate(HealthBarPrefab, worldCanvas.transform);
        GameObject platform = Instantiate(PlatformPrefab, transform);
        GameObject defenseStack = Instantiate(DefenseBuffStackPrefab, worldCanvas.transform);
        GameObject buffStack = Instantiate(BuffStackPrefab, worldCanvas.transform);
        GameObject debuffStack = Instantiate(DebuffStackPrefab, worldCanvas.transform);
        energyBar.transform.position = transform.position;
        healthBar.transform.position = transform.position;
        defenseStack.transform.position = transform.position;
        buffStack.transform.position = transform.position;
        debuffStack.transform.position = transform.position;
        EnergyBar = energyBar.GetComponent<EnergyBar>();
        HealthBar = healthBar.GetComponent<HealthBar>();
        Platform = platform.GetComponent<PlayerPlatform>();
        DefenseStack = defenseStack.GetComponent<BuffStack>();
        BuffStack = buffStack.GetComponent<BuffStack>();
        DebuffStack = debuffStack.GetComponent<BuffStack>();

        HealthBar.SetMaxHealth(BaseMaxHealth);
        HealthBar.SetHealth(BaseMaxHealth);
        Platform.SetTeam(Team);

        // Other bits
        Anim = GetComponent<Animator>();
        string sourceCode;
        if (Team.TeamNum == 1) sourceCode = FileManager.LoadFile("Player", ScriptFilename);
        else sourceCode = FileManager.LoadFile("Enemy", ScriptFilename);
        RuntimeInstance = new RuntimeInstance(sourceCode);
        RuntimeInstance.BindEnergyBar(EnergyBar);
        RuntimeInstance.BindCharacter(this);

        DamageMultipliers = new List<KeyValuePair<float, int>>();
        DefenseMultipliers = new List<KeyValuePair<float, int>>();

        currentHealth = BaseMaxHealth;
        charged = false;
        alive = true;

        BattleModel = BattleModel.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && BattleModel.instance.CurrentTimeStep > 0)
        {
            Damage(10);
        }
    }

    public void Step()
    {
        tookTurn = true;
        RuntimeInstance.Step();
        List<KeyValuePair<float, int>> toRemove = new List<KeyValuePair<float, int>>();
        // Decrement all multipliers and remove them if they have 0 time steps left
        int numPositive = 0;
        int numNegative = 0;
        for (int i = 0; i < DamageMultipliers.Count; i++)
        {
            DamageMultipliers[i] = new KeyValuePair<float, int>(DamageMultipliers[i].Key, DamageMultipliers[i].Value - 1);
            if (DamageMultipliers[i].Value <= 0) toRemove.Add(DamageMultipliers[i]);
            else if (DamageMultipliers[i].Key > 0) numPositive++;
            else numNegative++;
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
        if (DefenseStack != null)
            DefenseStack.ReDraw(DefenseMultipliers.Count);
        if (BuffStack != null)
            BuffStack.ReDraw(numPositive);
        if (DebuffStack != null)
            DebuffStack.ReDraw(numNegative);
    }

    public void AddDefenseMultiplier(float multiplier, int time)
    {
        if (!alive) return;
        DefenseMultipliers.Add(new KeyValuePair<float, int>(multiplier, time));
        DefenseStack.ReDraw(DefenseMultipliers.Count);
    }

    public void AddDamageMultiplier(float multiplier, int time)
    {
        DamageMultipliers.Add(new KeyValuePair<float, int>(multiplier, time));
        int positive = 0;
        int negative = 0;
        foreach (KeyValuePair<float, int> m in DamageMultipliers)
        {
            if (m.Key >= 0) positive++;
            else negative++;
        }
        if (BuffStack != null)
            BuffStack.ReDraw(positive);
        if (DebuffStack != null)
            DebuffStack.ReDraw(negative);
    }

    public void Damage(float amount)
    {
        if (!alive) return;
        float totalMultiplier = 1;
        foreach(KeyValuePair<float, int> multiplier in DefenseMultipliers)
        {
            totalMultiplier += multiplier.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        // Subtract adjusted amount from health and cap it at 0
        amount *= totalMultiplier;
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        HealthBar.SetHealth(currentHealth);
        GameObject worldCanvas = GameObject.Find("WorldCanvas");
        HealthText text = Instantiate(DamageTextPrefab, worldCanvas.transform).GetComponent<HealthText>();
        text.transform.position = transform.position;
        text.Init(-amount);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, BaseMaxHealth);

        HealthBar.SetHealth(currentHealth);
        GameObject worldCanvas = GameObject.Find("WorldCanvas");
        HealthText text = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity, worldCanvas.transform).GetComponent<HealthText>();
        text.transform.position = transform.position;
        text.Init(amount);
    }

    public void Die()
    {
        Anim.SetBool("IsDead", true);
        Anim.SetTrigger("Hurt");
        alive = false;
        tookTurn = false;
        EnergyBar.Hide();
        Platform.Hide();
        Destroy(EnergyBar.gameObject);
        Destroy(HealthBar.gameObject);
        Destroy(DefenseStack.gameObject);
        Destroy(BuffStack.gameObject);
        Destroy(DebuffStack.gameObject);
        BattleModel.RemoveCharacter(this);
        BattleModel.SetShouldReschedule();
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
        // If the character is taking a step this turn then make the enrgy bar visible, otherwise make it invisible
        if (tookTurn)
        {
            EnergyBar.Show();
            Platform.Show();
        }
        else
        {
            EnergyBar.Hide();
            Platform.Hide();
        }
        tookTurn = false;
        // Every character runs this function at the end of each turn. 
        if (currentHealth <= 0 && alive)
        {
            Die();
        }
    }

    public virtual void Attack(Value target)
    {
        Character player;
        try
        {
            player = target.GetAsPlayer().PlayerRef;

            float damage = BaseDamage;
            float totalMultiplier = 1;
            foreach (KeyValuePair<float, int> multipler in DamageMultipliers)
            {
                totalMultiplier += multipler.Key;
            }
            totalMultiplier = Mathf.Max(totalMultiplier, 0);
            damage *= totalMultiplier;
            player.Damage(damage);
            Anim.SetTrigger("Attack");
        } catch {
            return;
        }
    }

    public virtual void HealSelf()
    {
        float amount = BaseSelfHeal;
        float totalMultiplier = 1;
        foreach (KeyValuePair<float, int> multipler in DamageMultipliers)
        {
            totalMultiplier += multipler.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        Heal(amount);
        Anim.SetTrigger("Cast");
    }

    public virtual void DefendSelf()
    {
        float amount = BaseSelfDefend;
        float totalMultiplier = 1;
        foreach (KeyValuePair<float, int> multiplier in DamageMultipliers)
        {
            totalMultiplier += multiplier.Key;
        }
        totalMultiplier = Mathf.Max(totalMultiplier, 0);
        amount *= totalMultiplier;
        AddDefenseMultiplier(amount, 10);
        Anim.SetTrigger("Cast");
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
        Character target;
        try
        {
            Debug.Log("Sending " + message + " to " + player);
            target = player.GetAsPlayer().PlayerRef;
            if (target.IsAlive())
                target.ReceiveMessage(message.GetAsMessage());
        } catch
        {
            return;
        }
    }

    public virtual void SendMessageToAll(Value message)
    {
        try
        {
            MessageValue m = message.GetAsMessage();
            Debug.Log("Sending Message To All: " + m);
            BattleModel.SendMessageToAll(Team, m); 
        } catch
        {
            Debug.Log("An Error occured");
            return;
        }
    }

    public virtual void Listen()
    {
        Debug.Log("Listening");
    }

    public virtual void Yield()
    {
        BattleModel.SetShouldReschedule(Team);
        Debug.Log("Yielding");
    }

    public void ReceiveMessage(MessageValue message)
    {
        Debug.Log("Here: " + message + " " + RuntimeInstance);
        RuntimeInstance.ReceiveMessage(message);
        Debug.Log("Here2");
    }

    public override string ToString()
    {
        return "" + Team.TeamNum + CharacterNum;
    }

    public void OnGameEnd()
    {
        if (EnergyBar != null)
            Destroy(EnergyBar.gameObject);
        if (HealthBar != null)
            Destroy(HealthBar.gameObject);
        if (Platform != null)
            Destroy(Platform.gameObject);
        if (DefenseStack != null)
            Destroy(DefenseStack.gameObject);
        if (BuffStack != null)
            Destroy(BuffStack.gameObject);
        if (DebuffStack != null)
            Destroy(DebuffStack.gameObject);
        Anim.ResetTrigger("Attack");
        Anim.ResetTrigger("Cast");
        Anim.SetTrigger("Default");
    }
}
