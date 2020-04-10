using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FilterApply : MonoBehaviour
{
    List<string> axis1 = new List<string>();
    List<string> axis2 = new List<string>();
    string[] axis1arr;
    string[] axis2arr;
    int j = 0;
    Graph graph;
    public GameObject initGraph;
    Button button;
    GameObject apply;
    // Start is called before the first frame update
    void Start()
    {
        apply = this.gameObject;
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        initGraph = GameObject.FindGameObjectWithTag("environment");
        graph = initGraph.GetComponent<Graph>();
    }

    void TaskOnClick()
    {
        j = 0;
        for(int i = 0; i < Filters.axis1toggles.Count; i++)
        {
            Toggle toggle = Filters.axis1toggles[i].GetComponent<Toggle>();

            if(toggle.isOn)
            {
                axis2.Add(Filters.axis1toggles[i].name);
                j++;
            }
        }

        j = 0;
        for (int i = 0; i < Filters.axis2toggles.Count; i++)
        {
            Toggle toggle = Filters.axis2toggles[i].GetComponent<Toggle>();

            if (toggle.isOn)
            {
                axis1.Add(Filters.axis2toggles[i].name);
                j++;
            }
        }
        axis1arr = axis1.ToArray();
        axis2arr = axis2.ToArray();

        for(int i = 0; i < axis1arr.Length; i++)
        {
            Debug.Log(axis1arr[i]);
        }

        for (int i = 0; i < axis2arr.Length; i++)
        {
            Debug.Log(axis2arr[i]);
        }
        graph.axis1 = axis1arr;
        graph.axis2 = axis2arr;
        graph.generateBarChart(axis1arr, axis2arr);
    }
}
