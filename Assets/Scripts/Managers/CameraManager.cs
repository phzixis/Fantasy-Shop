using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public Transform topBound;
    public Transform botBound;

    Vector3 touchStart;
    Camera cam;
    bool isDragging = false;

    void Awake() {
        if (cam == null) {
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }
        if (Input.GetMouseButton(0) && isDragging) {
            Vector3 direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += Vector3.up * direction.y;

            float halfHeight = cam.orthographicSize;
            if(cam.transform.position.y + halfHeight > topBound.position.y) {
                cam.transform.position += Vector3.up * (topBound.position.y-halfHeight-cam.transform.position.y);
            } else if (cam.transform.position.y - halfHeight < botBound.position.y) {
                cam.transform.position += Vector3.up * (botBound.position.y+halfHeight-cam.transform.position.y);
            } 
         
        }
        if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }
    }
}
