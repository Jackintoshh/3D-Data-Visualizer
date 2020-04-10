using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    TextAsset data;

    public string[] axis1 = new string[0];
    public string[] axis2 = new string[0];
    //public List<string> axis1 = new List<string>();
    //public List<string> axis1 = new List<string>();
    int highRange;
    float spacing = 3.5f;
    List<Vector3> zpositions = new List<Vector3>();
    List<Vector3> xpositions = new List<Vector3>();
    public List<string> axis1toggles = new List<string>();
    public List<string> axis2toggles = new List<string>();
    
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

    // Start is called before the first frame update
    void Start()
    {
        //Load CSV File
        data = Resources.Load<TextAsset>("testdata");

        camera = Camera.main;
        camPos = new Vector3(-10f, 36f, -75f);
        camera.transform.position = camPos;

        graphPos = new GameObject();

        //Split data into separate lines
        string[] lines = data.text.Split(new char[] { '\n' });
        axis2 = lines[0].Split(new char[] { ',' });
        
        string[] temp = new string[6];

        //Get labels for XAxis
        for (int i = 1; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(new char[] { ',' });
            temp[i - 1] = split[0];
        }
        axis1 = temp;

        for (int i = 0; i < axis1.Length; i++)
        {
            axis2toggles.Add(axis1[i]);
        }

        for (int i = 0; i < axis2.Length; i++)
        {
            axis1toggles.Add(axis2[i]);
        }

        int k = 0;
        
        for (int i = 1; i < axis1.Length; i++)
        {
            k = 1;
            string[] values = lines[i].Split(new char[] { ',' });
            for (int j = 0; j < axis2.Length; j++)
            {
                dict.Add(axis1[i - 1] + ',' + axis2[j], values[k]);
                k++;
            }
        }

        foreach (KeyValuePair<string, string> item in dict)
        {
            Debug.Log(item);
        }
        generateBarChart(axis1, axis2);
        //generateScatterPlot(countries, years);
    }

    public void generateScatterPlot(string[] axis1, string[] axis2)
    {
        graphPos = this.gameObject;
        highRange = 100000;
        xpositions.Clear();
        zpositions.Clear();

        setupAxes(axis1, axis2);
        setupYAxis(axis1, axis2);

        //float ybounds = yaxis.GetComponent<Renderer>().bounds.size.y;
        ybounds = yaxis.transform.localScale.y;
        GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube);
        test.transform.position = new Vector3(0, ybounds, 0);
        Debug.Log(ybounds);

        //Get size of x and z axis to correctly place labels later
        xbounds = xaxis.GetComponent<Renderer>().bounds.max.y;
        zbounds = zaxis.GetComponent<Renderer>().bounds.max.y;

        //Divide axis length by amount of labels to get even distribution
        xbounds = xbounds / axis1.Length;
        zbounds = zbounds / axis2.Length;

        setupXAxis(axis1, axis2);
        setupZAxis(axis1, axis2);

        //Plot bars on the chart
        for (int i = 1; i < axis1.Length; i++)
        {
            //Get random colour for each country
            Color cubecol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            //Plot values for each country according to year
            for (int j = 1; j < axis2.Length; j++)
            {
                //Get x and z positions to plot on graph
                Vector3 sphereloc = new Vector3(xpositions[i - 1].x, 0, zpositions[j - 1].z);
                string key = axis1[i - 1] + ',' + axis2[j - 1];

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

                        sphere.name = axis1[i - 1] + ',' + axis2[j - 1] + ',' + value;

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

    
    void setupAxes(string[] axis1, string[] axis2)
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

        yaxis.transform.position = graphPos.transform.position;

        textloc = Vector3.zero;
    }

    void setupYAxis(string[] axis1, string[] axis2)
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

    void setupXAxis(string[] axis1, string[] axis2)
    {
        temp = 0;

        //Set up X-Axis
        for (int i = 0; i < axis1.Length; i++)
        {
            //Get correct text for labels
            GameObject xtext = new GameObject();
            xtext.transform.SetParent(graphPos.transform);
            xtext.AddComponent<TextMesh>().text = axis1[i];
            xtext.GetComponent<TextMesh>().characterSize = 2.0f;
            xtext.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;

            //Leave enough space for labels
            temp = temp + ((int)xbounds * 2);

            //Set text location on axis, remember that coordinate to use for later
            xtext.transform.position = new Vector3(temp, 0, 0);
            xpositions.Add(xtext.transform.position);

        }
    }

    void setupZAxis(string[] axis1, string[] axis2)
    {
        temp = 0;

        //Set up Z-Axis
        for (int i = 0; i < axis2.Length; i++)
        {
            //Get correct text for labels
            GameObject ztext = new GameObject();
            ztext.transform.SetParent(graphPos.transform);
            ztext.AddComponent<TextMesh>().text = axis2[i];
            ztext.GetComponent<TextMesh>().characterSize = 2.5f;

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

    

    public void generateBarChart(string[] axis1, string[] axis2)
    {
        graphPos = this.gameObject;
        highRange = 100000;
        xpositions.Clear();
        zpositions.Clear();

        setupAxes(axis1, axis2);
        setupYAxis(axis1, axis2);

        //float ybounds = yaxis.GetComponent<Renderer>().bounds.size.y;
        ybounds = yaxis.transform.localScale.y;
        GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube);
        test.transform.position = new Vector3(0, ybounds, 0);
        Debug.Log(ybounds);

        //Get size of x and z axis to correctly place labels later
        xbounds = xaxis.GetComponent<Renderer>().bounds.max.y;
        zbounds = zaxis.GetComponent<Renderer>().bounds.max.y;
        
        //Divide axis length by amount of labels to get even distribution
        xbounds = xbounds / axis1.Length;
        zbounds = zbounds / axis2.Length;

        setupXAxis(axis1, axis2);
        setupZAxis(axis1, axis2);


        //Plot bars on the chart
        for (int i = 1; i < axis1.Length; i++)
        {
            //Get random colour for each country
            Color cubecol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            //Plot values for each country according to year
            for (int j = 0; j < axis2.Length; j++)
                {
                    //Get x and z positions to plot on graph
                    Vector3 cubeloc = new Vector3(xpositions[i - 1].x, 0, zpositions[j].z);
                    string key = axis1[i-1] + ',' + axis2[j];
                
                    foreach (KeyValuePair<string, string> item in dict)
                    {
                        if(key == item.Key)
                        {
                            //Create bar
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.transform.SetParent(graphPos.transform);
                            cube.GetComponent<Renderer>().material.SetColor("_Color", cubecol);
                            cube.tag = "field";

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
