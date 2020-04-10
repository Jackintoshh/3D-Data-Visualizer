using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGraph : MonoBehaviour
{
    float RotateSpeed = 5f;
    Camera camera; 

    private void Start()
    {
        camera = Camera.main;
    }
    void OnMouseDrag()
    {
        transform.rotation = Quaternion.AngleAxis(-Input.GetAxis("Mouse X") * RotateSpeed, camera.transform.up) * transform.rotation;
        //Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * mouseRotateSpeed, camera.transform.right) *
        //transform.rotation;
    }
}
