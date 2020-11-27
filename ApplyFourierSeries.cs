using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ApplyFourierSeries : MonoBehaviour
{
    private Dictionary<int,Complex>[] coefficients;
    private Func<double,double[]> fourier_series;

    public LineRenderer line;

    double time=0.0d;
    private Vector3 temp_position;
    // Start is called before the first frame update
    public void CalculateFourierSeries(Func<double,double[]> function_to_transform, int max_frequency)
    {
        temp_position=transform.position;
        // int max_frequency=127;
        // double time_delta=1/max_frequency;
        //coefficients=FourierSeries.GetFourierCoefficients(FourierSeries.function,max_frequency,time_delta);
        coefficients=FourierSeries.fft_GetFourierCoefficients(function_to_transform,max_frequency);
        fourier_series=FourierSeries.GetFourierSeries(coefficients);
        foreach(Dictionary<int,Complex> dk in coefficients)
        {
            //Print all the coefficients for each dimension
            Debug.Log("For dimension");
            foreach(KeyValuePair<int,Complex> c in dk)
            {
                Debug.Log(c.Key+" : "+c.Value);
            }
            Debug.Log("\n");
        }

        line=GetComponent<LineRenderer>();
        line.positionCount=0;
        //line.width=0.04f;
        // Debug.Log(coefficients);
    }

    // Update is called once per frame
    public void AUpdate()
    {
        time+=0.001;
        double[] pos=fourier_series(time);
        // Debug.Log(pos[0]+" "+pos[1]+" "+pos[2]);
        temp_position.x=(float)pos[0];
        temp_position.y=(float)pos[1];
        temp_position.z=(float)pos[2]+0.1f;

        
        transform.position=temp_position;

        UpdateLineRenderer();
        
    }

    public void UpdateLineRenderer()
    {
        if(time<=1.01)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount-1,temp_position);
        }
    }
}
