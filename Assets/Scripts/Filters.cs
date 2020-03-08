using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Filters : MonoBehaviour
{

    public Button button;
    GameObject panel;
    TextMeshProUGUI text;
    Graph graphob;
    public GameObject filterToggle, applyButton;
    public GameObject togglepos, applypos;
    Toggle filToggle;
    RectTransform toggletrans;
    
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        panel = button.transform.parent.gameObject;
        graphob = panel.AddComponent<Graph>();
    }

    void TaskOnClick()
    {
        text = panel.GetComponentInChildren<TextMeshProUGUI>();
        
        for (int i = 0; i < Graph.years.Length; i++)
        {
            GameObject filToggle = Instantiate(filterToggle, button.transform.position, Quaternion.identity, panel.transform);
            
            Text toggletext = filToggle.GetComponentInChildren<Text>();
            
            toggletrans = filToggle.GetComponent<RectTransform>();
            filToggle.name = Graph.years[i];
            toggletrans.position = togglepos.transform.position;
            togglepos.transform.position += new Vector3(0f, -40f, 0f);
            
            
            /*GameObject toggle = new GameObject();
            toggle.transform.SetParent(panel.transform);
            filToggle = toggle.AddComponent<Toggle>();
            Text toggletext = toggle.GetComponentInChildren<Text>();
            */
            toggletext.text = Graph.years[i];
        }
        Instantiate(applyButton, applypos.transform.position, Quaternion.identity, panel.transform);
        
    }
}
