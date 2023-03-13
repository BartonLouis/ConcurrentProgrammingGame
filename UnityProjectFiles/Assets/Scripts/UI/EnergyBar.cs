using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{

    public GameObject EnergyBlockPrefab;
    public Transform Parent;
    public Vector3 Offset;

    private List<GameObject> EnergyBlocks;
    private int currentBlock;
    

    public void Start()
    {
        transform.position += Offset;
        EnergyBlocks = new List<GameObject>();
    }

    public void Setup(int numEnergyBlocks)
    {
        currentBlock = 0;
        foreach(GameObject block in EnergyBlocks)
        {
            Destroy(block);
        }
        EnergyBlocks.Clear();

        for(int i = 0; i < numEnergyBlocks; i++)
        {
            GameObject block = Instantiate(EnergyBlockPrefab, Parent);
            EnergyBlocks.Add(block);
        }
    }

    public void Step()
    {
        GameObject block = EnergyBlocks[currentBlock];
        Animator animator = block.GetComponent<Animator>();
        animator.SetTrigger("Full");
        currentBlock++;
    }

    public void Complete()
    {
        
        foreach(GameObject block in EnergyBlocks)
        {
            block.GetComponent<Animator>().SetTrigger("Active");
        }
    }



}
