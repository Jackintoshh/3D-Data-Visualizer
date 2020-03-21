using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighlightFirstCell : MonoBehaviour
{
    Button button;
    PreviewData pd;
    public GameObject content;
    GameObject firstCell;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        pd = content.GetComponent<PreviewData>();
        firstCell = pd.firstCell;
    }

    private void TaskOnClick()
    {
        text = pd.firstCell.GetComponent<TextMeshProUGUI>();
        text.text = "<mark=#ffff00aa>" + text.text + "</mark>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
