using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawFieldInfo : MonoBehaviour
{
    GameObject text;
    string info;
    GameObject globalCanvas;
    public Font font;

    void Awake()
    {
        globalCanvas = new GameObject();
        text = new GameObject();
    }

    public void drawInfo(RaycastHit hit)
    {
        info = hit.transform.gameObject.name;
        TextMeshProUGUI newText = GetComponent<TextMeshProUGUI>();
        info = info.Replace(',', '\n');
        Debug.Log(newText);
        newText.text = info;


    }
}
