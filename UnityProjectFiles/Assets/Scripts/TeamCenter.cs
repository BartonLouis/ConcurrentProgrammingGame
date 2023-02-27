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

    private float NumSpawns = 5;


    public void Init()
    {

        Players = new List<GameObject>();
        EmptySlots = new List<GameObject>();
        Reload();
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

    public void AddPlayer(ClassValue.ClassType classType)
    {
        GameObject player;
        switch (classType)
        {
            case ClassValue.ClassType.Damage:
                player = Instantiate(DamagePrefab, transform.position, Quaternion.identity);
                break;
            case ClassValue.ClassType.Support:
                player = Instantiate(SupportPrefab, transform.position, Quaternion.identity);
                break;
            default:
                player = Instantiate(TankPrefab, transform.position, Quaternion.identity);
                break;
        }
        player.transform.localScale = new Vector3(player.transform.localScale.x * direction, player.transform.localScale.y, player.transform.localScale.z);
        Players.Add(player);
        Reload();
    }

    public void UpdatePlayer(int index, ClassValue.ClassType classType)
    {
        Destroy(Players[index]); GameObject player;
        switch (classType)
        {
            case ClassValue.ClassType.Damage:
                player = Instantiate(DamagePrefab, transform.position, Quaternion.identity);
                break;
            case ClassValue.ClassType.Support:
                player = Instantiate(SupportPrefab, transform.position, Quaternion.identity);
                break;
            default:
                player = Instantiate(TankPrefab, transform.position, Quaternion.identity);
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
        float angle = (Mathf.PI/180) * 90 - direction * 90* (Mathf.PI/180);
        foreach(GameObject emptySlot in EmptySlots)
        {
            Destroy(emptySlot);
        }
        EmptySlots.Clear();

        for (int index = 0; index < NumSpawns; index ++){
            float x = Mathf.Cos(angle) * Xradius;
            float y = Mathf.Sin(angle) * Yradius;
            if (index < Players.Count)
            {
                Players[index].transform.position = new Vector2(transform.position.x + x, transform.position.y+y);
            } else
            {
                GameObject emptySlot = Instantiate(EmptySlotPrefab, transform.position, Quaternion.identity);
                emptySlot.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
                EmptySlots.Add(emptySlot);
            }
            angle += direction * nextAngle;
        }
        
    }
}
