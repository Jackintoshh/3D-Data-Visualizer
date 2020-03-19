using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreviewData : MonoBehaviour
{
    TextAsset data;
    int previewLength;
    GameObject panel;
    TextMeshProUGUI text;
    int i, j = 0;
    void Start()
    {
        previewLength = 5;
        data = Resources.Load<TextAsset>("testdata2");
        panel = this.gameObject;

        string[] lines = data.text.Split(new char[] { '\n' });
        
        
       // text = panel.GetComponentInChildren<TextMeshProUGUI>();
        
        //textOb.transform.position = panel.transform.position;

        /*for (int i = 0; i < previewLength; i++)
        {
            //text.text += lines[i] + '\n';
            string[] cells = lines[i].Split(new char[] { ',' });
            

            for (int j = 0; j < cells.Length; j++)
            {
                GameObject textOb = new GameObject();
                textOb.transform.SetParent(panel.transform);
                TextMeshProUGUI text = textOb.AddComponent<TextMeshProUGUI>();
                textOb.GetComponent<RectTransform>().sizeDelta = new Vector2(55, 50);
                text.text = cells[j];
                textOb.name = cells[j];
                text.fontSize = 12;

                if(j == cells.Length - 1)
                {
                    textOb.transform.position += new Vector3(0, 20, 0);
                    Debug.Log("Heya");
                }
            }
            
        }
        */

        for (int i = 0; i < previewLength; i++)
        {
            string[] cells = lines[i].Split(new char[] { ',' });
            GameObject textOb = new GameObject();
            textOb.transform.SetParent(panel.transform);
            TextMeshProUGUI text = textOb.AddComponent<TextMeshProUGUI>();
            HorizontalLayoutGroup hlg = textOb.AddComponent<HorizontalLayoutGroup>();
            hlg.childScaleHeight = true;
            hlg.childScaleWidth = true;
            hlg.childControlHeight = false;
            hlg.childControlWidth = false;
            hlg.childForceExpandHeight = false;
            hlg.childForceExpandWidth = false;
            hlg.padding.left = 55;
            textOb.GetComponent<RectTransform>().sizeDelta = new Vector2(55, 50);
            text.text = cells[0];
            text.fontSize = 12;

            
            for (int j = 1; j < cells.Length; j++)
            {
                GameObject hortextOb = new GameObject();
                hortextOb.transform.SetParent(textOb.transform);
                
                TextMeshProUGUI hortext = hortextOb.AddComponent<TextMeshProUGUI>();
                /*if (j == 1)
                {
                    hortextOb.GetComponent<RectTransform>().transform.position += new Vector3(20, 0, 0);
                }*/
                hortextOb.GetComponent<RectTransform>().sizeDelta = new Vector2(55, 50);
                hortext.text = cells[j];
                hortext.fontSize = 12;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
