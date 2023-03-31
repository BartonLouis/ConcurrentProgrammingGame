using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreLine : MonoBehaviour
{

    public int StepSize = 50;       // The number of pixels it will move the content to the left each step
    public int Spacing = 5;         // The amount of space to place between each block
    public Transform ContentBox;    // The Content Box to move and place blocks inside
    public GameObject BlockPrefab;  // The Blocks prefab to add
    public float moveSpeed = 1;

    private int TurnSize;
    private System.Random Rnd;
    private Vector3 TargetPosition = Vector3.zero;
    private Vector3 StartPosition;

    public void Awake()
    {
        ContentBox.GetComponent<HorizontalLayoutGroup>().spacing = Spacing;
        ContentBox.GetComponent<HorizontalLayoutGroup>().padding.left = StepSize / 2;
        TurnSize = StepSize - Spacing;
        Rnd = new System.Random();
    }

    public void Start()
    {
        StartPosition = ContentBox.transform.localPosition;
    }

    public void AddBlock(int turns, Character character)
    {
        GameObject block = Instantiate(BlockPrefab, ContentBox);
        block.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, turns * TurnSize + (turns - 1) * Spacing);
        block.GetComponent<StepBlock>().SetCharacter(character);
    }

    public void AddBlock(int turns)
    {
        AddBlock(turns, null);
    }

    private void Update()
    {
        if (TargetPosition != Vector3.zero)
        {
            float distance = (ContentBox.transform.localPosition - TargetPosition).magnitude;
            if (distance < 0.01)
            {
                ContentBox.transform.localPosition = TargetPosition;
            } else
            {
                ContentBox.transform.localPosition += (TargetPosition - ContentBox.transform.localPosition).normalized * moveSpeed * Time.deltaTime;
            }
        }
    }

    public void Step(int currentTimeStep)
    {
        // ContentBox.transform.position = new Vector3(ContentBox.position.x - StepSize, ContentBox.position.y, ContentBox.position.z);
        if (TargetPosition != Vector3.zero)
        {
            ContentBox.transform.localPosition = TargetPosition;
        } else
        {
            TargetPosition = StartPosition;
        }
        TargetPosition = new Vector3(StartPosition.x - currentTimeStep * StepSize, ContentBox.localPosition.y, ContentBox.localPosition.z);
        float distance = (ContentBox.transform.localPosition - TargetPosition).magnitude;
        if (GameController.instance.CurrentSpeed > 0) moveSpeed = distance * GameController.instance.CurrentSpeed;
        else moveSpeed = distance / 1;
    }

    public void Reset(List<KeyValuePair<Character, int>> representation)
    {
        foreach(Transform child in ContentBox)
        {
            Destroy(child.gameObject);
        }
        foreach (KeyValuePair<Character, int> block in representation)
        {
            if (block.Key == null) AddBlock(block.Value);
            else AddBlock(block.Value, block.Key);
        }
    }

}
