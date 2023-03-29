using System.Collections.Generic;
using UnityEngine;
using Interpreter;

public class TeamCenter : MonoBehaviour
{
    private List<GameObject> Players;
    private List<GameObject> EmptySlots;

    public float Xradius = 10f;
    public float Yradius = 2f;
    public int direction = -1;

    public int TeamNum;
    private float NumSpawns = 5;

    private List<GameObject> ChargePoints;


    public void Init()
    {
        Players = new List<GameObject>();
        EmptySlots = new List<GameObject>();
        Reload();

        // Spawn charge points
        float nextAngle = 2 * Mathf.PI / Mathf.Max(2, NumSpawns);
        // First Character should be at the front
        float angle = nextAngle / 2;
        ChargePoints = new List<GameObject>();
        for (int i = 0; i < Mathf.Max(2, NumSpawns); i++) {
            float x = Mathf.Cos(angle + (Mathf.PI / 180) * 90 - direction * 90 * (Mathf.PI / 180)) * Xradius;
            float y = Mathf.Sin(angle + (Mathf.PI / 180) * 90 - direction * 90 * (Mathf.PI / 180)) * Yradius;
            GameObject chargePoint = Instantiate(PrefabLibrary.instance.ChargePointPrefab, transform.position, Quaternion.identity);
            chargePoint.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
            ChargePoints.Add(chargePoint);
            if (angle <= 0)
            {
                angle = nextAngle - angle;
            }
            else
            {
                angle = -angle;
            }
        }
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
                player = Instantiate(PrefabLibrary.instance.DamagePrefab, transform.position, Quaternion.identity);
                break;
            case ClassValue.ClassType.Support:
                player = Instantiate(PrefabLibrary.instance.SupportPrefab, transform.position, Quaternion.identity);
                break;
            default:
                player = Instantiate(PrefabLibrary.instance.TankPrefab, transform.position, Quaternion.identity);
                break;
        }
        Character c = player.GetComponent<Character>();
        c.Team = this;
        c.ScriptFilename = filename;
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
                player = Instantiate(PrefabLibrary.instance.DamagePrefab, transform.position, Quaternion.identity);
                break;
            case ClassValue.ClassType.Support:
                player = Instantiate(PrefabLibrary.instance.SupportPrefab, transform.position, Quaternion.identity);
                break;
            default:
                player = Instantiate(PrefabLibrary.instance.TankPrefab, transform.position, Quaternion.identity);
                break;
        }
        Character c = player.GetComponent<Character>();
        c.Team = this;
        c.ScriptFilename = filename;
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
                Character c = Players[index].GetComponent<Character>();
                // Set the left and right chargePoints of that character
                if (index % 2 == 0)
                {
                    c.SetLeft(ChargePoints[(int)Mathf.Min(index + 1, NumSpawns - 1)].GetComponent<ChargePoint>());
                    c.SetRight(ChargePoints[(int)Mathf.Max(0, index - 1)].GetComponent<ChargePoint>());
                } else
                {
                    c.SetRight(ChargePoints[(int)Mathf.Min(index + 1, NumSpawns - 1)].GetComponent<ChargePoint>());
                    c.SetLeft(ChargePoints[(int)Mathf.Max(0, index - 1)].GetComponent<ChargePoint>());
                }
            } else
            {
                GameObject emptySlot = Instantiate(PrefabLibrary.instance.EmptySlotPrefab, transform.position, Quaternion.identity);
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

    public bool AllDead()
    {
        List<Character> characters = GetCharacters();
        foreach(Character c in characters)
        {
            if (c.IsAlive())
                return false;
        }
        return true;
    }

    public void OnBattleBegin()
    {
        foreach (GameObject chargePoint in ChargePoints)
        {
            chargePoint.GetComponent<ChargePoint>().OnBattleBegin();
        }
    }

    public void OnBattleEnd()
    {
        foreach(GameObject chargePoint in ChargePoints)
        {
            chargePoint.GetComponent<ChargePoint>().OnBattleEnd();
        }
    }
}
