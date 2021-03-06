﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Graph : MonoBehaviour
{
    //TextAsset data;
    string data;

    //public string[] axis1 = new string[0];
    //public string[] axis2 = new string[0];
    public List<string> axis1 = new List<string>();
    public List<string> axis2 = new List<string>();
    int highRange;
    float spacing;
    List<Vector3> zpositions = new List<Vector3>();
    List<Vector3> xpositions = new List<Vector3>();
    public List<string> axis1toggles = new List<string>();
    public List<string> axis2toggles = new List<string>();
    string path;
    
    float unitConverter;
    float axisSize;
    Dictionary<string, string> dict = new Dictionary<string, string>();
    public GameObject graphPos;
    Camera camera;
    public Vector3 camPos;
    public float mouseRotateSpeed = 5f;

    GameObject yaxis;
    GameObject xaxis;
    GameObject zaxis;

    int intervals;
    int temp;
    Vector3 textloc;
    float ybounds, xbounds, zbounds;
    float textSize;

    // Start is called before the first frame update
    void Start()
    {
        path = PreviewData.path1;

        //Load CSV File
        data = System.IO.File.ReadAllText(path);//Resources.Load<TextAsset>("testdata");

        camera = Camera.main;
        camPos = new Vector3(-10f, 36f, -75f);
        camera.transform.position = camPos;

        graphPos = new GameObject();

        //Split data into separate lines
        string[] lines = data.Split(new char[] { '\n' });
        Debug.Log(lines[0]);
        string[] temp = Regex.Split(lines[0], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        for (int i = 1; i < temp.Length; i++)
        {
            
        axis2.Add(temp[i]);//lines[0].Split(new char[] { ',' });
        }

        for (int i = 0; i < axis2.Count; i++)
        {
            Debug.Log(axis2[i]);
        }
        string[] temp2 = new string[6];

        //Get labels for XAxis
        for (int i = 1; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(new char[] { ',' });
            axis1.Add(split[0]);
        }
        //axis1 = temp;

        for (int i = 0; i < axis1.Count; i++)
        {
            axis2toggles.Add(axis1[i]);
        }

        for (int i = 0; i < axis2.Count; i++)
        {
            axis1toggles.Add(axis2[i]);
        }

        int k = 0;
        
        for (int i = 1; i < axis1.Count; i++)
        {
            k = 1;
            string[] values = lines[i].Split(new char[] { ',' });
            for (int j = 0; j < axis2.Count; j++)
            {
                Debug.Log(axis2[j]);
                Debug.Log(values[k]);
                dict.Add(axis1[i - 1] + ',' + axis2[j], values[k]);
                k++;
            }
        }

        foreach (KeyValuePair<string, string> item in dict)
        {
            Debug.Log(item);
        }

        if(PreviewData.dropdown.value == 0)
        {
            generateBarChart(axis1, axis2);
            
        }

        if (PreviewData.dropdown.value == 1)
        {
            generateScatterPlot(axis1, axis2);
        }

        if (PreviewData.dropdown.value == 2)
        {
            generateConnectedDots(axis1, axis2);
        }

        if (PreviewData.dropdown.value == 3)
        {
            generateSeriesPlot(axis1, axis2);
        }
    }

    public void generateScatterPlot(List<string> axis1, List<string> axis2)
    {
        graphPos = this.gameObject;
        highRange = 100000;
        spacing = 3f;
        xpositions.Clear();
        zpositions.Clear();
        textSize = 1.8f;

        setupAxes(axis1, axis2);
        setupYAxis(axis1, axis2);

        //float ybounds = yaxis.GetComponent<Renderer>().bounds.size.y;
        ybounds = yaxis.transform.localScale.y;

        //Get size of x and z axis to correctly place labels later
        xbounds = xaxis.GetComponent<Renderer>().bounds.max.y;
        zbounds = zaxis.GetComponent<Renderer>().bounds.max.y;

        //Divide axis length by amount of labels to get even distribution
        xbounds = xbounds / axis1.Count;
        zbounds = zbounds / axis2.Count;

        setupXAxis(axis1, axis2);
        setupZAxis(axis1, axis2);

        //Plot bars on the chart
        for (int i = 1; i < axis1.Count; i++)
        {
            //Get random colour for each country
            Color cubecol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            //Plot values for each country according to year
            for (int j = 0; j < axis2.Count; j++)
            {
                //Get x and z positions to plot on graph
                Vector3 sphereloc = new Vector3(xpositions[i - 1].x, 0, zpositions[j].z);
                string key = axis1[i - 1] + ',' + axis2[j];

                foreach (KeyValuePair<string, string> item in dict)
                {
                    if (key == item.Key)
                    {
                        //Create bar
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.SetParent(graphPos.transform);
                        sphere.GetComponent<Renderer>().material.SetColor("_Color", cubecol);
                        sphere.tag = "field";


                        //Get value of field
                        int value = System.Convert.ToInt32(item.Value);

                        sphere.name = axis1[i - 1] + ',' + axis2[j] + ',' + value;

                        //Scale bar based on value
                        sphere.transform.position = sphereloc;
                        sphere.transform.localScale += new Vector3(3, 3, 3); //Divide value by 1338 to get value in Unity units
                        sphere.transform.position = new Vector3(sphereloc.x, value/unitConverter, sphereloc.z);
                    }
                }

            }
        }

        //graphPos.transform.child
        //Set axes in correct locations + rotations
        setupAxisOrientation();
    }

    
    void setupAxes(List<string> axis1, List<string> axis2)
    {
        foreach (Transform child in graphPos.transform)
        {
            Destroy(child.gameObject);
        }


        //Set amount of labels on Y axis
        intervals = 10;

        //Generate x,y,z axis
        yaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        xaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        zaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);


        yaxis.transform.SetParent(graphPos.transform);
        xaxis.transform.SetParent(graphPos.transform);
        zaxis.transform.SetParent(graphPos.transform);

        axisSize = yaxis.GetComponent<Renderer>().bounds.size.y + (intervals * spacing) * 2;
        unitConverter = highRange / axisSize;
        camPos = new Vector3(camPos.x, axisSize / 2, camPos.z);
        camera.transform.position = camPos;

        //Get correct value to increment by
        highRange = highRange / intervals;

        yaxis.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        xaxis.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        zaxis.GetComponent<Renderer>().material.SetColor("_Color", Color.black);

        textloc = new Vector3(graphPos.transform.position.x - 10, graphPos.transform.position.y + 2, graphPos.transform.position.z);
    }

    void setupYAxis(List<string> axis1, List<string> axis2)
    {
        for (int i = 0; i < intervals; i++)
        {
            //Increase size of axis for every label
            yaxis.transform.localScale += new Vector3(0, spacing, 0);

            //Set text content and size of text
            GameObject ytext = new GameObject();
            ytext.transform.SetParent(graphPos.transform);
            ytext.AddComponent<TextMesh>().text = temp.ToString();
            ytext.GetComponent<TextMesh>().characterSize = 2.5f;

            temp += highRange;

            //Set location of labels on axis
            ytext.GetComponent<TextMesh>().transform.position = textloc;

            //Spaces labels based on conversion from Numbers to Unity units
            textloc += new Vector3(0, (int)highRange / unitConverter, 0);

            //Set size of other axes 
            xaxis.transform.localScale += new Vector3(0, spacing, 0);
            zaxis.transform.localScale += new Vector3(0, spacing, 0);
        }
    }

    void setupXAxis(List<string> axis1, List<string> axis2)
    {
        temp = 0;

        //Set up X-Axis
        for (int i = 0; i < axis1.Count; i++)
        {
            //Get correct text for labels
            GameObject xtext = new GameObject();
            xtext.transform.SetParent(graphPos.transform);
            xtext.AddComponent<TextMesh>().text = axis1[i];
            xtext.GetComponent<TextMesh>().characterSize = textSize;
            xtext.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;

            //Leave enough space for labels
            temp = temp + ((int)xbounds * 2);

            //Set text location on axis, remember that coordinate to use for later
            xtext.transform.position = new Vector3(temp, 0, 0);
            xpositions.Add(xtext.transform.position);

        }
    }

    void setupZAxis(List<string> axis1, List<string> axis2)
    {
        temp = 0;

        //Set up Z-Axis
        for (int i = 0; i < axis2.Count; i++)
        {
            //Get correct text for labels
            GameObject ztext = new GameObject();
            ztext.transform.SetParent(graphPos.transform);
            ztext.AddComponent<TextMesh>().text = axis2[i];
            ztext.GetComponent<TextMesh>().characterSize = textSize;

            //Leave enough space for labels
            temp = temp + ((int)zbounds * 2);

            //Set position of label, remember this coordinate for later
            ztext.transform.position = new Vector3(0, 0, temp);
            zpositions.Add(ztext.transform.position);

        }
    }

    void setupAxisOrientation()
    {
        yaxis.transform.position += new Vector3(0, yaxis.transform.localScale.y, 0);
        xaxis.transform.Rotate(0, 0, 90f);
        zaxis.transform.Rotate(90f, 0, 0);
        xaxis.transform.position += new Vector3(xaxis.transform.localScale.y, 0, 0);
        zaxis.transform.position += new Vector3(0, 0, zaxis.transform.localScale.y);
    }

    

    public void generateBarChart(List<string> axis1, List<string> axis2)
    {
        graphPos = this.gameObject;
        highRange = 100000;
        xpositions.Clear();
        zpositions.Clear();
        textSize = 2f;

        spacing = 3.8f;
        setupAxes(axis1, axis2);
        setupYAxis(axis1, axis2);

        
        ybounds = yaxis.transform.localScale.y;
        

        //Get size of x and z axis to correctly place labels later
        xbounds = xaxis.GetComponent<Renderer>().bounds.max.y;
        zbounds = zaxis.GetComponent<Renderer>().bounds.max.y;
        
        //Divide axis length by amount of labels to get even distribution
        xbounds = xbounds / axis1.Count;
        zbounds = zbounds / axis2.Count;

        setupXAxis(axis1, axis2);
        setupZAxis(axis1, axis2);


        //Plot bars on the chart
        for (int i = 1; i < axis1.Count; i++)
        {
            //Get random colour for each country
            Color cubecol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            //Plot values for each country according to year
            for (int j = 0; j < axis2.Count; j++)
                {
                    //Get x and z positions to plot on graph
                    Vector3 cubeloc = new Vector3(xpositions[i - 1].x, 0, zpositions[j].z);
                    string key = axis1[i-1] + ',' + axis2[j];
                    Debug.Log(key);
                    foreach (KeyValuePair<string, string> item in dict)
                    {
                        if(key == item.Key)
                        {
                            //Create bar
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.transform.SetParent(graphPos.transform);
                            cube.GetComponent<Renderer>().material.SetColor("_Color", cubecol);
                            cube.tag = "field";

                            Debug.Log(item.Value + "here");
                            //Get value of field
                            int value = System.Convert.ToInt32(item.Value);

                            cube.name = axis1[i - 1] + ',' + axis2[j] + ',' + value;

                            //Scale bar based on value
                            cube.transform.position = cubeloc;
                            cube.transform.localScale += new Vector3(3, value / unitConverter, 3); //Divide value by 1338 to get value in Unity units
                            cube.transform.position += new Vector3(0, cube.transform.localScale.y / 2, 0);
                        }
                    }
                
                }
        }

        //graphPos.transform.child
        //Set axes in correct locations + rotations
        setupAxisOrientation();

        

    }

    public void generateConnectedDots(List<string> axis1, List<string> axis2)
    {
        graphPos = this.gameObject;
        highRange = 100000;
        spacing = 4f;
        xpositions.Clear();
        zpositions.Clear();
        textSize = 2f;

        setupAxes(axis1, axis2);
        setupYAxis(axis1, axis2);

        //float ybounds = yaxis.GetComponent<Renderer>().bounds.size.y;
        ybounds = yaxis.transform.localScale.y;

        //Get size of x and z axis to correctly place labels later
        xbounds = xaxis.GetComponent<Renderer>().bounds.max.y;
        zbounds = zaxis.GetComponent<Renderer>().bounds.max.y;

        //Divide axis length by amount of labels to get even distribution
        xbounds = xbounds / axis1.Count;
        zbounds = zbounds / axis2.Count;

        setupXAxis(axis1, axis2);
        setupZAxis(axis1, axis2);

        
        //Plot bars on the chart
        for (int i = 1; i < axis1.Count; i++)
        {
            //Get random colour for each country
            Color cubecol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Vector3[] linepositions = new Vector3[axis2.Count];

            //Plot values for each country according to year
            for (int j = 0; j < axis2.Count; j++)
            {
                //Get x and z positions to plot on graph
                Vector3 lineDotloc = new Vector3(xpositions[i - 1].x, 0, zpositions[j].z);
                string key = axis1[i - 1] + ',' + axis2[j];
                
                //line.positionCount = axis2.Count;
                foreach (KeyValuePair<string, string> item in dict)
                {
                    if (key == item.Key)
                    {
                        //Create bar
                        GameObject lineDot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        lineDot.transform.SetParent(graphPos.transform);
                        lineDot.GetComponent<Renderer>().material.SetColor("_Color", cubecol);
                        lineDot.tag = "field";

                        //LineRenderer line = lineDot.AddComponent<LineRenderer>();
                        

                        //Get value of field
                        int value = System.Convert.ToInt32(item.Value);

                        lineDot.name = axis1[i - 1] + ',' + axis2[j] + ',' + value;

                        //Scale bar based on value
                        lineDot.transform.position = lineDotloc;
                        lineDot.transform.localScale += new Vector3(1f, 1f, 1f); //Divide value by 1338 to get value in Unity units
                        lineDot.transform.position = new Vector3(lineDotloc.x, value / unitConverter, lineDotloc.z);
                        linepositions[j] = new Vector3(lineDotloc.x, value / unitConverter, lineDotloc.z);
                        //
                    }
                }
                
            }
            GameObject lineOb = new GameObject();
            lineOb.transform.SetParent(graphPos.transform);
            LineRenderer line = lineOb.AddComponent<LineRenderer>();
            line.positionCount = axis2.Count - 1;
            line.GetComponent<Renderer>().material.SetColor("_Color", cubecol);
            line.alignment = LineAlignment.View;
            line.useWorldSpace = false;
            line.SetPositions(linepositions);
        }

        //graphPos.transform.child
        //Set axes in correct locations + rotations
        setupAxisOrientation();
    }

    public void generateSeriesPlot(List<string> axis1, List<string> axis2)
    {
        graphPos = this.gameObject;
        highRange = 100000;
        xpositions.Clear();
        zpositions.Clear();
        textSize = 1.5f;

        spacing = 2.5f;
        setupAxes(axis1, axis2);
        setupYAxis(axis1, axis2);


        ybounds = yaxis.transform.localScale.y;


        //Get size of x and z axis to correctly place labels later
        xbounds = xaxis.GetComponent<Renderer>().bounds.max.y;
        zbounds = zaxis.GetComponent<Renderer>().bounds.max.y;

        //Divide axis length by amount of labels to get even distribution
        xbounds = xbounds / axis1.Count;
        zbounds = zbounds / axis2.Count;

        setupXAxis(axis1, axis2);
        setupZAxis(axis1, axis2);


        //Plot bars on the chart
        for (int i = 1; i < axis1.Count; i++)
        {
            //Get random colour for each country
            Color cubecol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            //Plot values for each country according to year
            for (int j = 0; j < axis2.Count; j++)
            {
                //Get x and z positions to plot on graph
                Vector3 cubeloc = new Vector3(xpositions[i - 1].x, 0, zpositions[j].z);
                string key = axis1[i - 1] + ',' + axis2[j];
                Debug.Log(key);
                foreach (KeyValuePair<string, string> item in dict)
                {
                    if (key == item.Key)
                    {
                        //Create bar
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.SetParent(graphPos.transform);
                        cube.GetComponent<Renderer>().material.SetColor("_Color", cubecol);
                        cube.tag = "field";

                        Debug.Log(item.Value + "here");
                        //Get value of field
                        int value = System.Convert.ToInt32(item.Value);

                        cube.name = axis1[i - 1] + ',' + axis2[j] + ',' + value;

                        //Scale bar based on value
                        cube.transform.position = cubeloc;
                        cube.transform.localScale += new Vector3(0.1f, value / unitConverter, 4); //Divide value by 1338 to get value in Unity units
                        cube.transform.position += new Vector3(0, cube.transform.localScale.y / 2, 0);
                    }
                }

            }
        }

        //graphPos.transform.child
        //Set axes in correct locations + rotations
        setupAxisOrientation();
    }

}
