using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;

public class MoveAround : MonoBehaviour
{
    public Camera mainCamera;
    public float speed = 50.0f;
    public bool allowPitch = false;
    public static RaycastHit hit;
    Ray ray;
    public GameObject canvas;
    public float mouseRotateSpeed = 15f;
    public GameObject camText;
    [SerializeField]
    public bool camMode;
    Graph graph;
    RotateGraph rg;
    public GameObject graphob;
    LayerMask mask;
    DrawFieldInfo dfi;

    // Use this for initialization
    void Start()
    {
       
        mainCamera = Camera.main;
        mask = LayerMask.GetMask("spin");
        dfi = canvas.AddComponent<DrawFieldInfo>();
        camMode = true;
        graph = graphob.GetComponent<Graph>();
    }

  
  

    void Walk(float units)
    {
        transform.position += mainCamera.transform.forward * units;
    }

    void Fly(float units)
    {
        transform.position += Vector3.up * units;
    }

    void Strafe(float units)
    {
        transform.position += mainCamera.transform.right * units;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX, mouseY;
        float speed = this.speed;

        float runAxis = 0; // Input.GetAxis("Run Axis");


        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)&& hit.collider.tag == "field")
        {
            dfi.drawInfo(hit);
        }


        if (Input.GetKey(KeyCode.E))
        {
            Fly(Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.F))
        {
            Fly(-Time.deltaTime * speed);
        }
        
        if (Input.GetKeyDown("h"))
        {
            if(camMode)
            {
                camMode = false;
                camText.GetComponent<TextMeshProUGUI>().text = "Free Cam";
            }
            else if(!camMode)
            {
                camMode = true;
                camText.GetComponent<TextMeshProUGUI>().text = "Fixed Cam";
            }
            
        }
        
        Vector3 a = new Vector3();
        a.x = 10;

        if (camMode)
        {
            mainCamera.transform.position = graph.camPos;
        }
        else
        {


            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");


            //Yaw(mouseX * speed * Time.deltaTime);
            if (allowPitch)
            {
                //Pitch(-mouseY * speed * Time.deltaTime);
            }

            float contWalk = Input.GetAxis("Vertical");
            float contStrafe = Input.GetAxis("Horizontal");
            Walk(contWalk * speed * Time.deltaTime);
            Strafe(contStrafe * speed * Time.deltaTime);
       }
    }

    
}