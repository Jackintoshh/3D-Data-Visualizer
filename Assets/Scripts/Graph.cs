using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    TextAsset data;
    string[] countries = new string[6];
    int highRange = 100000;
    float spacing = 3.5f;
    List<Vector3> zpositions = new List<Vector3>();
    List<Vector3> xpositions = new List<Vector3>();
    float unitConverter;
    float axisSize;

    // Start is called before the first frame update
    void Start()
    {
        //Load CSV File
        data = Resources.Load<TextAsset>("testdata");
        
        //Split data into separate lines + get all years needed to be plotted
        string[] lines = data.text.Split(new char[] { '\n' });
        string[] years = lines[0].Split(new char[] { ',' });
        //unitConverter = highRange / axisSize; // 72 is length of the axis in Unity units
        
        //Get all countries
        for (int i = 1; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(new char[] { ',' });
            countries[i-1] = split[0];
        }

        generateBarChart(countries, years, lines);
    }

    void generateBarChart(string[] countries, string[] years, string[] lines)
    {
        GameObject graphPos = new GameObject();
        graphPos = this.gameObject;
        
        //Set amount of labels on Y axis
        int intervals = 10;

        

        

        
        //Generate x,y,z axis
        GameObject yaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        GameObject xaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        GameObject zaxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        axisSize = yaxis.GetComponent<Renderer>().bounds.size.y + (intervals * spacing) * 2;
        unitConverter = highRange / axisSize;
        Debug.Log(axisSize);

        //Get correct value to increment by
        highRange = highRange / intervals;
        int temp = 0;

        yaxis.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        xaxis.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        zaxis.GetComponent<Renderer>().material.SetColor("_Color", Color.black);

        yaxis.transform.position = graphPos.transform.position;

        Vector3 textloc = Vector3.zero;

        //Set up Y-Axis
        for (int i = 0; i < intervals; i++)
        {
            //Increase siz of axis for every label
            yaxis.transform.localScale += new Vector3(0, spacing, 0);

            //Set text content and size of text
            GameObject ytext = new GameObject();
            ytext.AddComponent<TextMesh>().text = temp.ToString();
            ytext.GetComponent<TextMesh>().characterSize = 2.5f;

            temp += highRange;
            
            //Set location of labels on axis
            ytext.GetComponent<TextMesh>().transform.position = textloc;

            //Spaces labels based on conversion from Numbers to Unity units
            textloc += new Vector3(0, (int)highRange/unitConverter, 0); 

            //Set size of other axes 
            xaxis.transform.localScale += new Vector3(0, spacing, 0);
            zaxis.transform.localScale += new Vector3(0, spacing, 0);
        }


        //float ybounds = yaxis.GetComponent<Renderer>().bounds.size.y;
        float ybounds = yaxis.transform.localScale.y;
        GameObject test = GameObject.CreatePrimitive(PrimitiveType.Cube);
        test.transform.position = new Vector3(0, ybounds, 0);
        Debug.Log(ybounds);

        //Get size of x and z axis to correctly place labels later
        float xbounds = xaxis.GetComponent<Renderer>().bounds.max.y;
        float zbounds = zaxis.GetComponent<Renderer>().bounds.max.y;
        
        //Divide axis length by amount of labels to get even distribution
        xbounds = xbounds / countries.Length;
        zbounds = zbounds / years.Length;
        
        temp = 0;

        //Set up X-Axis
        for(int i = 0; i < countries.Length; i++)
        {
            //Get correct text for labels
            GameObject xtext = new GameObject();
            xtext.AddComponent<TextMesh>().text = countries[i];
            xtext.GetComponent<TextMesh>().characterSize = 2.0f;
            xtext.GetComponent<TextMesh>().anchor = TextAnchor.MiddleLeft;

            //Leave enough space for labels
            temp = temp + ((int)xbounds*2);

            //Set text location on axis, remember that coordinate to use for later
            xtext.transform.position = new Vector3(temp, 0, 0);
            xpositions.Add(xtext.transform.position);
        }

        temp = 0;

        //Set up Z-Axis
        for(int i = 0; i < years.Length; i++)
        {
            //Get correct text for labels
            GameObject ztext = new GameObject();
            ztext.AddComponent<TextMesh>().text = years[i];
            ztext.GetComponent<TextMesh>().characterSize = 2.5f;

            //Leave enough space for labels
            temp = temp + ((int)zbounds * 2);

            //Set position of label, remember this coordinate for later
            ztext.transform.position = new Vector3(0, 0, temp);
            zpositions.Add(ztext.transform.position);
        }

        //Plot bars on the chart
        for (int i = 1; i < countries.Length; i++)
        {
            //Get each value to be plotted
            string[] values = lines[i].Split(new char[] { ',' });

            //Get random colour for each country
            Color cubecol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            //Plot values for each country according to year
            for (int j = 1; j < years.Length; j++)
            {
                //Get x and z positions to plot on graph
                Vector3 cubeloc = new Vector3(xpositions[i-1].x, 0, zpositions[j-1].z);

                //Create bar
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.GetComponent<Renderer>().material.SetColor("_Color", cubecol);
                cube.tag = "field";
                

                //Get value of field
                int value = System.Convert.ToInt32(values[j]);

                cube.name = countries[i - 1] + ',' + years[j - 1] + ',' + value;

                //Scale bar based on value
                cube.transform.position = cubeloc;
                cube.transform.localScale += new Vector3(3, value / unitConverter, 3); //Divide value by 1338 to get value in Unity units
                cube.transform.position += new Vector3(0, cube.transform.localScale.y/2, 0);
            }
        }

        //Set axes in correct locations + rotations
        yaxis.transform.position += new Vector3(0, yaxis.transform.localScale.y, 0);
        xaxis.transform.Rotate(0, 0, 90f);
        zaxis.transform.Rotate(90f, 0, 0);
        xaxis.transform.position += new Vector3(xaxis.transform.localScale.y, 0, 0);
        zaxis.transform.position += new Vector3(0, 0, zaxis.transform.localScale.y);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
