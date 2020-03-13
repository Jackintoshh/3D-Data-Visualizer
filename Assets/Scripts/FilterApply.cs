using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FilterApply : MonoBehaviour
{
    List<string> countries = new List<string>();
    List<string> years = new List<string>();
    string[] countriesarr;
    string[] yearsarr;
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
        button.onClick.AddListener(TaskOnClick2);
        initGraph = GameObject.FindGameObjectWithTag("environment");
        graph = initGraph.GetComponent<Graph>();
    }

    void TaskOnClick2()
    {
        j = 0;
        for(int i = 0; i < Filters.axis1toggles.Count; i++)
        {
            Toggle toggle = Filters.axis1toggles[i].GetComponent<Toggle>();

            if(toggle.isOn)
            {
                years.Add(Filters.axis1toggles[i].name);
                j++;
            }
        }

        j = 0;
        for (int i = 0; i < Filters.axis2toggles.Count; i++)
        {
            Toggle toggle = Filters.axis2toggles[i].GetComponent<Toggle>();

            if (toggle.isOn)
            {
                countries.Add(Filters.axis2toggles[i].name);
                j++;
            }
        }
        countriesarr = countries.ToArray();
        yearsarr = years.ToArray();
        graph.generateBarChart(countriesarr, yearsarr);
    }
}
