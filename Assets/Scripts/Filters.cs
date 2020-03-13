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
    //Graph graphob;
    public GameObject filterToggle, applyButton;
    public GameObject togglepos, applypos;
    public GameObject initGraph;
    Graph graph;
    RectTransform toggletrans;
    public static List<GameObject> axis1toggles = new List<GameObject>();
    public static List<GameObject> axis2toggles = new List<GameObject>();
    bool isClicked;
    Vector3 origTogglePos;
    
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        panel = button.transform.parent.gameObject;
        //graphob = panel.AddComponent<Graph>();
        graph = initGraph.GetComponent<Graph>();
        isClicked = false;
        origTogglePos = togglepos.transform.position;
    }

    void TaskOnClick()
    {
        if (!isClicked)
        {
            text = panel.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log("Heya");
            axis1toggles.Clear();
            axis2toggles.Clear();
            togglepos.transform.position = origTogglePos;

            for (int i = 0; i < graph.years.Length; i++)
            {
                axis1toggles.Add(Instantiate(filterToggle, button.transform.position, Quaternion.identity, panel.transform));

                Text toggletext = axis1toggles[i].GetComponentInChildren<Text>();

                toggletrans = axis1toggles[i].GetComponent<RectTransform>();
                axis1toggles[i].name = graph.years[i];
                toggletrans.transform.position = togglepos.transform.position;
                togglepos.transform.position += new Vector3(0f, -40f, 0f);


                /*GameObject toggle = new GameObject();
                toggle.transform.SetParent(panel.transform);
                filToggle = toggle.AddComponent<Toggle>();
                Text toggletext = toggle.GetComponentInChildren<Text>();
                */
                toggletext.text = graph.years[i];
                
            }

            for (int i = 0; i < graph.countries.Length; i++)
            {
                if (graph.countries[i] != null)
                {

                    axis2toggles.Add(Instantiate(filterToggle, togglepos.transform.position, Quaternion.identity, panel.transform));

                    Text toggletext = axis2toggles[i].GetComponentInChildren<Text>();

                    toggletrans = axis2toggles[i].GetComponent<RectTransform>();
                    axis2toggles[i].name = graph.countries[i];
                    toggletrans.position = togglepos.transform.position;
                    togglepos.transform.position += new Vector3(0f, -40f, 0f);


                    /*GameObject toggle = new GameObject();
                    toggle.transform.SetParent(panel.transform);
                    filToggle = toggle.AddComponent<Toggle>();
                    Text toggletext = toggle.GetComponentInChildren<Text>();
                    */
                    toggletext.text = graph.countries[i];
                    
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
