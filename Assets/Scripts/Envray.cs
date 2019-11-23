using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envray : MonoBehaviour
{
    Transform cam;
    public GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        Ray envray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        

        if(Input.GetMouseButton(0))
        {
            if (Physics.Raycast(envray, out hit))
            {
                if(hit.collider.gameObject.tag == "environment")
                {
                    Debug.Log("HIT");
                    Instantiate(plane, hit.point, Quaternion.identity);
                }
            }
        }
        
    }
}
