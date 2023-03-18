using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStack : MonoBehaviour
{
    [SerializeField] GameObject BuffPrefab;
    [SerializeField] Vector3 Offset;

    private void Start()
    {
        transform.position += Offset;
    }

    public void ReDraw(int numBuffs)
    {
        Debug.Log("Redrawing: " + numBuffs);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < numBuffs; i++)
        {
            Instantiate(BuffPrefab, transform);
        }
    }
    

}
