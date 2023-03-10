using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class TeamCenter : MonoBehaviour
{
    private List<GameObject> Players;
    private List<GameObject> EmptySlots;
    public GameObject EmptySlotPrefab;

    public GameObject DamagePrefab;
    public GameObject SupportPrefab;
    public GameObject TankPrefab;

    public float Xradius = 10f;
    public float Yradius = 2f;
    public int direction = -1;

    public int TeamNum;
    private float NumSpawns = 5;


    public void Init()
    {
        Players = new List<GameObject>();
        EmptySlots = new List<GameObject>();
        Reload();
    }

    public void SetTeamNum(int num)
    {
        TeamNum = num;
    }

    public void SetNumSpawns(int numSpawns)
    {
        NumSpawns = numSpawns;
    }

    public bool IsFull(int minPlayers, int maxPlayers)
    {
        return (Players.Count >= minPlayers && Players.Count <= maxPlayers);
    }

    public void RemovePlayer(int index)
    {
        Destroy(Players[index]);
        Players.RemoveAt(index);
        Reload();
    }

    public List<Character> GetCharacters() {
        List<Character> characters = new List<Character>();
        int index = 0;
        foreach (GameObject player in Players){
            player.GetComponent<Character>().CharacterNum = index;
            characters.Add(player.GetComponent<Character>());
            index++;
        }
        return characters;
    }

    public void AddPlayer(ClassValue.ClassType classType, string filename)
    {
        GameObject player;
        switch (classType)
        {
            case ClassValue.ClassType.Damage:
                player = Instantiate(DamagePrefab, transform.position, Quaternion.identity);
                player.GetComponent<Damage>().Team = this;
                player.GetComponent<Damage>().ScriptFilename = filename;
                break;
            case ClassValue.ClassType.Support:
                player = Instantiate(SupportPrefab,  transform.position, Quaternion.identity);
                player.GetComponent<Support>().Team = this;
                player.GetComponent<Support>().ScriptFilename = filename;
                break;
            default:
                player = Instantiate(TankPrefab, transform.position, Quaternion.identity);
                player.GetComponent<Tank>().Team = this;
                player.GetComponent<Tank>().ScriptFilename = filename;
                break;
        }
        player.transform.localScale = new Vector3(player.transform.localScale.x * direction, player.transform.localScale.y, player.transform.localScale.z);
        Players.Add(player);
        Reload();
    }

    public void UpdatePlayer(int index, ClassValue.ClassType classType, string filename)
    {
        if (classType == ClassValue.ClassType.Any)
        {
            classType = Players[index].GetComponent<Character>().ClassType;
        }
        Destroy(Players[index]);
        GameObject player;
        switch (classType)
        {
            case ClassValue.ClassType.Damage:
                player = Instantiate(DamagePrefab, transform.position, Quaternion.identity);
                player.GetComponent<Damage>().Team = this;
                player.GetComponent<Damage>().ScriptFilename = filename;
                break;
            case ClassValue.ClassType.Support:
                player = Instantiate(SupportPrefab, transform.position, Quaternion.identity);
                player.GetComponent<Support>().Team = this;
                player.GetComponent<Support>().ScriptFilename = filename;
                break;
            default:
                player = Instantiate(TankPrefab, transform.position, Quaternion.identity);
                player.GetComponent<Tank>().Team = this;
                player.GetComponent<Tank>().ScriptFilename = filename;
                break;
        }
        player.transform.localScale = new Vector3(player.transform.localScale.x * direction, player.transform.localScale.y, player.transform.localScale.z);
        Players[index] = player;
        Reload();
    }

    void Reload()
    {
        float nextAngle = 2 * Mathf.PI / NumSpawns;
        // First Character should be at the front
        float angle = 0;
        foreach(GameObject emptySlot in EmptySlots)
        {
            Destroy(emptySlot);
        }
        EmptySlots.Clear();

        for (int index = 0; index < NumSpawns; index ++){
            float x = Mathf.Cos(angle + (Mathf.PI / 180) * 90 - direction * 90 * (Mathf.PI / 180)) * Xradius;
            float y = Mathf.Sin(angle + (Mathf.PI / 180) * 90 - direction * 90 * (Mathf.PI / 180)) * Yradius;
            if (index < Players.Count)
            {
                Players[index].transform.position = new Vector2(transform.position.x + x, transform.position.y+y);
            } else
            {
                GameObject emptySlot = Instantiate(EmptySlotPrefab, transform.position, Quaternion.identity);
                emptySlot.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
                EmptySlots.Add(emptySlot);
            }
            
            if (angle <= 0)
            {
                angle = nextAngle - angle;
            } else
            {
                angle = -angle;
            }
            // angle += direction * nextAngle;
        }
    }
}
