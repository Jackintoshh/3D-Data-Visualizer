using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawFieldInfo : MonoBehaviour
{
    string info;
    public Font font;

    public void drawInfo(RaycastHit hit)
    {
        info = hit.transform.gameObject.name;
        TextMeshProUGUI newText = GetComponent<TextMeshProUGUI>();
        info = info.Replace(',', '\n');
        newText.text = info;
    }
}
