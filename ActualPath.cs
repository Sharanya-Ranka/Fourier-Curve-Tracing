using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActualPath : MonoBehaviour
{
    private Func<double,double[]> actual_path;
    public LineRenderer line;

    double time=0.1;
    private Vector3 temp_position;
    // Start is called before the first frame update
    void Start()
    {
        line=GetComponent<LineRenderer>();
        line.positionCount=0;
        //line.width=0.04f;
    }

    // Update is called once per frame
    void Update()
    {
        time+=0.001;
        if(time>10)
            time=0;
        double[] pos=FourierSeries.function(time);
        // Debug.Log(pos[0]+" "+pos[1]+" "+pos[2]);
        temp_position.x=(float)(50*Math.Sin(5*time));
        temp_position.y=(float)(50*Math.Cos(5*time));
        temp_position.z=(float)(50*Math.Sin(5*time));

        
        transform.position=temp_position;
        line.positionCount++;
        line.SetPosition(line.positionCount-1,temp_position);
    }
}
