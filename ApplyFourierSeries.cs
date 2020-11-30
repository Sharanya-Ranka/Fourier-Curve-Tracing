using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ApplyFourierSeries : MonoBehaviour
{
    private Dictionary<int,Complex>[] coefficients;
    private Func<double,double[]> fourier_series;

    public LineRenderer line;

    double time=0.0d, time_delta = 0.001d;
    private Vector3 temp_position;
    // Start is called before the first frame update
    public void CalculateFourierSeries(Func<double,double[]> function_to_transform, int max_frequency)
    {
        /*
        Calculates fourier series of function_to_transform using max_frequency.
        Uses FourierSeries.cs to accomplish task. Stores the fourier series to be used later for
        showing the approximation
        */

        temp_position=transform.position;
        coefficients=FourierSeries.fft_GetFourierCoefficients(function_to_transform,max_frequency);
        fourier_series=FourierSeries.GetFourierSeries(coefficients);

        line=GetComponent<LineRenderer>();
        line.positionCount=0;

    }

    public void FunctionUpdate()
    {
        /*
        Updates the position of the GameObject this script is attached to. 
        Uses the fourier series approximation to update position
        */
        time += time_delta;
        double[] pos=fourier_series(time);

        temp_position.x=(float)pos[0];
        temp_position.y=(float)pos[1];
        temp_position.z=(float)pos[2]+0.1f;

        transform.position=temp_position;

        UpdateLineRenderer();
        
    }

    public void UpdateLineRenderer()
    {
        /*
        Updates the line renderer of the object, so that the approximation can be seen.
        */
        if(time <= 1 + time_delta)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount-1,temp_position);
        }
    }
}
