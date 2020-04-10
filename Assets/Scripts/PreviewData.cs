using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using SimpleFileBrowser;

public class PreviewData : MonoBehaviour
{
    TextAsset data;
    int previewLength;
    GameObject panel;
    public GameObject firstCell;
    TextMeshProUGUI text;
    int CellSize;
    public GameObject filebutton;
    public GameObject gobutton;
    Button Filebutton;
    
    void Start()
    {
        previewLength = 5;
        CellSize = 200;
        data = Resources.Load<TextAsset>("testdata3");
        string data2 = data.ToString();
        Debug.Log(data2);
        panel = this.gameObject;
        Debug.Log("Hello");
        string[] lines = data.text.Split(new char[] { '\n' });
        Filebutton = filebutton.GetComponent<Button>();
        Filebutton.onClick.AddListener(TaskOnClick);

        for (int i = 0; i < previewLength; i++)
        {
            string[] cells = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            
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
            hlg.padding.left = CellSize;
            textOb.GetComponent<RectTransform>().sizeDelta = new Vector2(CellSize, 50);
            text.text = cells[0];
            text.fontSize = 24;
            text.color = Color.black;

            if (i == 0)
            {
                firstCell = textOb;
            }

            for (int j = 1; j < cells.Length; j++)
            {
                GameObject hortextOb = new GameObject();
                hortextOb.transform.SetParent(textOb.transform);
                
                TextMeshProUGUI hortext = hortextOb.AddComponent<TextMeshProUGUI>();
                /*if (j == 1)
                {
                    hortextOb.GetComponent<RectTransform>().transform.position += new Vector3(20, 0, 0);
                }*/
                hortextOb.GetComponent<RectTransform>().sizeDelta = new Vector2(CellSize, 50);
                hortext.text = cells[j];
                hortext.color = Color.black;
                hortext.fontSize = 24;
            }

        }

    }


    void TaskOnClick()
    {
        FileBrowser.SetDefaultFilter(".csv");

        FileBrowser.ShowLoadDialog((path) => { Debug.Log("Selected: " + path); },
                                        () => { Debug.Log( "Canceled" ); }, 
                                        false, null, "Select Folder", "Select" );
    }
}
