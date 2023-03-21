using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementParralax : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        cameraTransform.position += new Vector3(.01f, 0);
    }
}
