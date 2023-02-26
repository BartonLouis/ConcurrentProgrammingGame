using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{

    private Camera cam;
    private int UILayer;
    private Vector3 dragOrigin;

    public float zoomSpeed = 2;
    public float minSize=2;
    public float maxSize=10;
    private bool dragging = true;

    private float MinX, MinY, MaxX, MaxY;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject background = GameObject.Find("ArenaBackground");
        SpriteRenderer image = background.GetComponent<SpriteRenderer>();
        MinX = image.transform.position.x - image.bounds.size.x / 2f;
        MaxX = image.transform.position.x + image.bounds.size.x / 2f;
        MinY = image.transform.position.y - image.bounds.size.y / 2f;
        MaxY = image.transform.position.y + image.bounds.size.y / 2f;

        cam = Camera.main;
        UILayer = LayerMask.NameToLayer("UI");
    }

    private void LateUpdate()
    {
        PanCamera();
        ZoomCamera();
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;
        }

        if (Input.GetMouseButton(0) && dragging)
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        } else
        {
            dragging = false;
        }
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        for (int index = 0; index < raysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = raysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    private void ZoomCamera()
    {
        float amount = Input.GetAxis("Mouse ScrollWheel");
        float newSize = cam.orthographicSize - amount * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float minX = MinX + camWidth;
        float maxX = MaxX - camWidth;
        float minY = MinY + camHeight;
        float maxY = MaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
