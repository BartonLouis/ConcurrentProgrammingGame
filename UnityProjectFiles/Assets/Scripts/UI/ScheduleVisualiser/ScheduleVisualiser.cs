using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleVisualiser : MonoBehaviour
{
    public static ScheduleVisualiser instance;

    [SerializeField] Transform Parent;
    [SerializeField] GameObject CoreLinePrefab;

    CoreLine[] CoreLines;

    private Animator Animator;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void Setup(int numCores)
    {
        Animator.SetBool("Open", true);
        CoreLines = new CoreLine[numCores];
        for(int i = 0; i < numCores; i++)
        {
            GameObject coreLine = Instantiate(CoreLinePrefab, Parent);
            CoreLines[i] = coreLine.GetComponent<CoreLine>();
        }
    }

    public void BattleEnd()
    {
        Animator.SetBool("Open", false);
        foreach(CoreLine c in CoreLines)
        {
            Destroy(c.gameObject);
        }
    }

    public void AddBlock(int coreIndex, Character character, int steps)
    {
        if (character == null)
        {
            CoreLines[coreIndex].AddBlock(steps);
        }
        else
        {
            CoreLines[coreIndex].AddBlock(steps, character);
        }
    }
    public void Step(int currentTimeStep)
    {
        foreach(CoreLine core in CoreLines)
        {
            core.Step(currentTimeStep);
        }
    }

    public void Reset(List<List<KeyValuePair<Character, int>>> representation)
    {
        Debug.Log("Resetting");
        for (int i = 0; i < representation.Count; i++)
        {
            CoreLines[i].Reset(representation[i]);
        }
    }
}
