using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyBar : MonoBehaviour
{

    public GameObject EnergyBlockPrefab;
    public Transform Parent;
    public Vector3 Offset;
    public Color VisibleColour = new Color(0, 0, 0, 0.3f);
    public Color HiddenColour = new Color(0, 0, 0, 0);
    public TextMeshProUGUI Text;

    private List<GameObject> EnergyBlocks;
    private int currentBlock;
    private int mode = 0;
    

    public void Start()
    {
        mode = 0;
        transform.position += Offset;
        EnergyBlocks = new List<GameObject>();
        Text.enabled = false;
    }

    public void Setup(string text)
    {
        Debug.Log("here");
        mode = 1;
        Text.enabled = true;
        Text.text = text;
        foreach (GameObject block in EnergyBlocks)
        {
            Destroy(block);
        }
        EnergyBlocks.Clear();
    }

    public void Setup(int numEnergyBlocks)
    {
        Debug.Log("here2");
        mode = 0;
        Text.enabled = false;
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
        if (mode == 0)
        {
            GameObject block = EnergyBlocks[currentBlock];
            Animator animator = block.GetComponent<Animator>();
            animator.SetBool("Full", true);
            currentBlock++;
        }
    }

    public void Complete()
    {
        if (mode == 0)
        {
            foreach (GameObject block in EnergyBlocks)
            {
                block.GetComponent<Animator>().SetBool("Active", true);
                block.GetComponent<Animator>().SetBool("Full", false);
            }
        }
    }

    public void Show()
    {
        if (mode == 0)
        {
            foreach (GameObject block in EnergyBlocks)
            {
                block.GetComponent<Animator>().ResetTrigger("Hide");
                block.GetComponent<Animator>().SetTrigger("Show");
            }
        }
    }

    public void Hide()
    {
        if (mode == 0)
        {
            foreach (GameObject block in EnergyBlocks)
            {
                block.GetComponent<Animator>().ResetTrigger("Show");
                block.GetComponent<Animator>().SetTrigger("Hide");
            }
        } 
    }



}
