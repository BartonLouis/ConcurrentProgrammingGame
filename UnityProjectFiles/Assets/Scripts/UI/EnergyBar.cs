using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{

    public GameObject EnergyBlockPrefab;
    public Transform Parent;
    public Vector3 Offset;
    public Color VisibleColour = new Color(0, 0, 0, 0.3f);
    public Color HiddenColour = new Color(0, 0, 0, 0);

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
            foreach (Transform child in block.transform)
            {
                if (child.name == "Mask") child.GetComponent<Image>().color = HiddenColour;
            }
            EnergyBlocks.Add(block);
        }
    }

    public void Step()
    {
        GameObject block = EnergyBlocks[currentBlock];
        Animator animator = block.GetComponent<Animator>();
        animator.SetBool("Full", true);
        currentBlock++;
    }

    public void Complete()
    {
        
        foreach(GameObject block in EnergyBlocks)
        {
            block.GetComponent<Animator>().SetBool("Active", true);
            block.GetComponent<Animator>().SetBool("Full", false);
        }
    }

    public void Show()
    {
        foreach (GameObject block in EnergyBlocks)
        {
            block.GetComponent<Animator>().ResetTrigger("Hide");
            block.GetComponent<Animator>().SetTrigger("Show");
        }
    }

    public void Hide()
    {
        foreach (GameObject block in EnergyBlocks)
        {
            block.GetComponent<Animator>().ResetTrigger("Show");
            block.GetComponent<Animator>().SetTrigger("Hide");
        }
    }



}
