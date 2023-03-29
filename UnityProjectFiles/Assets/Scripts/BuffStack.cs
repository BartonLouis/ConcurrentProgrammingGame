using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStack : MonoBehaviour
{
    [SerializeField] Vector3 Offset;

    private GameObject Prefab;

    private void Start()
    {
        transform.position += Offset;
    }

    public void Init(GameObject prefab)
    {
        Prefab = prefab;
    }

    public void ReDraw(int numBuffs)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < numBuffs; i++)
        {
            Instantiate(Prefab, transform);
        }
    }
    

}
