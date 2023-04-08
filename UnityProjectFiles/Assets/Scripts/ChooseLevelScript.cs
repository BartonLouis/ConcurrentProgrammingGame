using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLevelScript : MonoBehaviour
{
    public Transform NextButton;
    // Start is called before the first frame update
    void Start()
    {
        if (NextButton == null) return;
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
        lr.SetPosition(1, new Vector3(NextButton.position.x, NextButton.position.y, 0));
    }
}
