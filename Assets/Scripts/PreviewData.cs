using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using SimpleFileBrowser;

public class PreviewData : MonoBehaviour
{
    string data;
    int previewLength;
    GameObject panel;
    public GameObject firstCell;
    int CellSize;

    public GameObject filebutton;
    public GameObject gobutton;
    public GameObject dropdownob;

    Button Filebutton;
    public static string path1;

    public static TMP_Dropdown dropdown;


    void Start()
    {
        previewLength = 5;
        CellSize = 200;
        //data = Resources.Load<TextAsset>("testdata3");
        //string data2 = data.ToString();
        panel = this.gameObject;
        
        Filebutton = filebutton.GetComponent<Button>();
        Filebutton.onClick.AddListener(TaskOnClick);

         dropdown = dropdownob.GetComponent<TMP_Dropdown>();
        

        

    }


    void TaskOnClick()
    {
        FileBrowser.SetDefaultFilter(".csv");

        FileBrowser.ShowLoadDialog((path) => {getPath(path); },
                                        () => { Debug.Log( "Canceled" ); }, 
                                        false, null, "Select Folder", "Select" );

        

    }

    void getPath(string path)
    {
        path1 = path;
        Debug.Log(path1);
        data = System.IO.File.ReadAllText(path1);

        for (int i = 0; i < previewLength; i++)
        {
            string[] lines = data.Split(new char[] { '\n' });
            string[] cells = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            GameObject textOb = new GameObject();
            textOb.transform.SetParent(panel.transform);

            //Set layout group parameters
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

                hortextOb.GetComponent<RectTransform>().sizeDelta = new Vector2(CellSize, 50);
                hortext.text = cells[j];
                hortext.color = Color.black;
                hortext.fontSize = 24;
            }

        }
    }
}
