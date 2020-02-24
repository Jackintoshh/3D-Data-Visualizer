using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawFieldInfo : MonoBehaviour
{
    GameObject text;
    GameObject globalCanvas;
    public Font font;

    void Awake()
    {
        globalCanvas = new GameObject();
        text = new GameObject();
    }

    public void drawInfo(RaycastHit hit)
    {
      
        //text.transform.SetParent(this.transform);
        //text.name = "info";
        
        Text newText = GetComponent<Text>();
        
        newText.text = hit.transform.gameObject.name;


    }
}
