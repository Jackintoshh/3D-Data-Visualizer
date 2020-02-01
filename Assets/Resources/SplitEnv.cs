using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnv : MonoBehaviour
{
    float gridcountX = 5.0f;
    float gridcountZ = 5.0f;
    float maxheight, maxwidth, minheight, minwidth;
    public GameObject env, line; 
    public List<GameObject> lineList1 = new List<GameObject>();
    public List<GameObject> lineList2 = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        env = this.gameObject;
        
        maxheight = env.GetComponent<Renderer>().bounds.max.x;
        maxwidth = env.GetComponent<Renderer>().bounds.max.z;
        minheight = env.GetComponent<Renderer>().bounds.min.x;
        minwidth = env.GetComponent<Renderer>().bounds.min.z;
        
        LineRenderer[] lines1 = new LineRenderer[5];
        LineRenderer[] lines2 = new LineRenderer[5];
        float startz = calcMinMax(minwidth, maxwidth, gridcountZ);
        float startx = calcMinMax(minheight, maxheight, gridcountX);
        //lines along Z axis
        for (int i = 0; i<gridcountZ-1; i++)
        {
            lineList1.Add((GameObject)Instantiate(line, env.transform.position, Quaternion.identity));
            lines1[i] = lineList1[i].GetComponent<LineRenderer>();
            //lines[i].positionCount = 4;
            lines1[i].SetPosition(0, new Vector3(minheight, 0.0f, startz));
            lines1[i].SetPosition(1, new Vector3(maxheight, 0.0f, startz));
            startz = startz + 20;
        }
        //lines along X axis
        for (int i = 0; i < gridcountX - 1; i++)
        {
            lineList2.Add((GameObject)Instantiate(line, env.transform.position, Quaternion.identity));
            lines2[i] = lineList2[i].GetComponent<LineRenderer>();
            //lines[i].positionCount = 4;
            lines2[i].SetPosition(0, new Vector3(startx, 0.0f, minwidth));
            lines2[i].SetPosition(1, new Vector3(startx, 0.0f, maxwidth));
            startx = startx + 20;
        }



        Debug.Log("height: " + maxheight + " maxwidth: " + maxwidth + " minwidth" + minwidth);
        
        Debug.Log("startz: " + startz);
        
        //drawlines.positionCount = 20;
        
        /*for (int i = 0; i < 5; i++)
        {
            drawlines.SetPosition(i, new Vector3(minheight, 0.0f, startz));
            drawlines.SetPosition(i + 1, new Vector3(maxheight, 0.0f, startz));
            startz += 20;
        }*/
        //Vector3 trans = new Vector3(gameObject.transform);
        /*drawlines.SetPosition(0, new Vector3(minheight, 0.0f, startz));
        drawlines.SetPosition(1, new Vector3(maxheight, 0.0f, startz));
        drawlines.SetPosition(2, new Vector3(minheight, 0.0f, startz + 20));
        drawlines.SetPosition(3, new Vector3(maxheight, 0.0f, startz + 20));
        */
        Debug.Log("x: " + gameObject.transform.position.x + "y: " + gameObject.transform.position.y + "z: " + gameObject.transform.position.z);
        //Debug.Log("height: " + maxheight + " maxwidth: " + maxwidth + " minwidth" + minwidth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float calcMinMax(float min, float max, float gridcount)
    {
        Debug.Log("height: " + max + " minwidth" + min);
        float diff = max - min;
        float start = diff / gridcount;
        min = min + start;
        Debug.Log("Start: " + start);
        return min;

    }
}
