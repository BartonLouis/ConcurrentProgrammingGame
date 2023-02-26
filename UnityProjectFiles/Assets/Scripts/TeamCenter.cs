using System.Collections.Generic;
using UnityEngine;

public class TeamCenter : MonoBehaviour
{
    private List<GameObject> Players;
    private List<GameObject> EmptySlots;
    public GameObject PlayerPrefab;
    public GameObject EmptySlotPrefab;
    public float radius = 5f;
    public float numSpawns = 5;
    public int direction = -1;


    private void Start()
    {
        Players = new List<GameObject>();
        EmptySlots = new List<GameObject>();
        Reload();
    }

    public void RemovePlayer(int index)
    {
        Destroy(Players[index]);
        Players.RemoveAt(index);
        Reload();
    }

    public void AddPlayer()
    {
        GameObject player = Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        Players.Add(player);
        Reload();
    }

    void Reload()
    {
        float nextAngle = 2 * Mathf.PI / numSpawns;
        // First Character should be at the front
        float angle = (Mathf.PI/180) * 90 - direction * 90* (Mathf.PI/180);
        foreach(GameObject emptySlot in EmptySlots)
        {
            Destroy(emptySlot);
        }
        EmptySlots.Clear();

        for (int index = 0; index < numSpawns; index ++){
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Debug.Log("Here");
            if (index < Players.Count)
            {
                Debug.Log("Here 2");
                Players[index].transform.position = new Vector2(transform.position.x + x, transform.position.y+y);
            } else
            {
                Debug.Log("Here 3");
                GameObject emptySlot = Instantiate(EmptySlotPrefab, transform.position, Quaternion.identity);
                emptySlot.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
                EmptySlots.Add(emptySlot);
            }
            angle += direction * nextAngle;
        }
        
    }
}
