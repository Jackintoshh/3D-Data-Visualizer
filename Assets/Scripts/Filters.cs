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
    public GameObject filterToggle, applyButton;
    public GameObject togglepos, applypos;
    public GameObject initGraph;
    Graph graph;
    public static List<GameObject> axis1toggles = new List<GameObject>();
    public static List<GameObject> axis2toggles = new List<GameObject>();
    bool isClicked;
    public GameObject content;
    
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        panel = button.transform.parent.gameObject;
        graph = initGraph.GetComponent<Graph>();
        isClicked = false;
    }

    void TaskOnClick()
    {
        if (!isClicked)
        {
            text = panel.GetComponentInChildren<TextMeshProUGUI>();
            axis1toggles.Clear();
            axis2toggles.Clear();

            for (int i = 0; i < graph.axis1toggles.Count; i++)
            {
                axis1toggles.Add(Instantiate(filterToggle, button.transform.position, Quaternion.identity, panel.transform));
                axis1toggles[i].transform.SetParent(content.transform);
                Text toggletext = axis1toggles[i].GetComponentInChildren<Text>();
                axis1toggles[i].name = graph.axis1toggles[i];
                toggletext.text = graph.axis1toggles[i];
                
            }

            for (int i = 0; i < graph.axis2toggles.Count; i++)
            {
                if (graph.axis1[i] != null)
                {

                    axis2toggles.Add(Instantiate(filterToggle, togglepos.transform.position, Quaternion.identity, panel.transform));
                    axis2toggles[i].transform.SetParent(content.transform);
                    Text toggletext = axis2toggles[i].GetComponentInChildren<Text>();
                    axis2toggles[i].name = graph.axis2toggles[i];
                    toggletext.text = graph.axis2toggles[i];
                    
                }
            }

            Instantiate(applyButton, applypos.transform.position, Quaternion.identity, panel.transform);
            isClicked = true;
        }
        else
        {
            foreach (GameObject child in axis1toggles)
            {
                Destroy(child.gameObject);
            }

            foreach (GameObject child in axis2toggles)
            {
                Destroy(child.gameObject);
            }

            isClicked = false;
        }
        
    }
}
