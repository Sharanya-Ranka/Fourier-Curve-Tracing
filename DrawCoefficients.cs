using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DrawCoefficients : MonoBehaviour
{
    private Dictionary<int,Complex>[] coefficients;

    public GameObject Prefab;
    public GameObject ArrowPrefab;
    public List<GameObject> availableObjects = new List<GameObject>();
    public List<GameObject> availableArrows = new List<GameObject>();

    double time=0.0d, time_delta = 0.001d;
    private static double pi=3.14159265;

    private bool visibility = true;

    private Vector3 temp_position;
    // Start is called before the first frame update
    public void CalculateFourierSeries(Func<double,double[]> function_to_transform, int max_frequency)
    {
        /*
        Gets the coefficients of the fourier series of function_to_transform.
        Is used in making the arrows that track the cumulative movement of each frequency.
        */

        coefficients=FourierSeries.fft_GetFourierCoefficients(function_to_transform,max_frequency);
        
        //Instantiating required GameObjects

        for(int i=0;i<(coefficients[0].Count+1)/2;i++)
        {
            availableObjects.Add(Instantiate(Prefab,new Vector3(0, 0, 0), Quaternion.identity));
            availableArrows.Add(Instantiate(ArrowPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        }
    }

    public void FunctionUpdate()
    {
        time+= time_delta;

        Vector3 prev_pos = new Vector3(0,0,0);
        int component_count = (coefficients[0].Count+1)/2;

        for(int go = 0; go < component_count; go++)
        {
            Vector3 pos = new Vector3(0,0,0);
            for(int dim=0;dim<3;dim++)
            {
                Complex t = new Complex();
                Complex multiplier_norm=Complex.FromPolarCoordinates(1.0d,2*pi*go*time);
                t+=multiplier_norm*coefficients[dim][go]*2;
                if(go != 0)
                {
                    Complex multiplier_conj=Complex.FromPolarCoordinates(1.0d,-2*pi*go*time);
                    t+=multiplier_conj*coefficients[dim][-go]*2;
                }
                pos[dim] = (float)t.Real;
            }
            //Fixing position, rotation and scale of the arrow corresponding to frequency 'go'
            availableArrows[go].transform.position = prev_pos;
            float scaleby_float = Vector3.Distance(prev_pos, prev_pos+pos);
            Vector3 scaleby = new Vector3(scaleby_float, scaleby_float, scaleby_float);
            availableArrows[go].transform.localScale = scaleby;
            availableArrows[go].transform.LookAt(prev_pos+pos);

            prev_pos += pos;
        }

        // Updating the position and line renderer of the last coefficient 
        // Used to find out which curve the set of arrows are tracing
        availableObjects[component_count-1].transform.position= prev_pos;
        UpdateLineRenderer(component_count-1, prev_pos);
    }

    public void UpdateLineRenderer(int game_obj_ind, Vector3 pos)
    {
        /*
        Updates line renderer of the particular game object.
        Adds position pos to the list
        */
        LineRenderer line = availableObjects[game_obj_ind].GetComponent<LineRenderer>();
        if(time<=1 + time_delta)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount-1,pos);
        }
    }

    public void ToggleVisibility()
    {
        visibility = ! visibility;

        for(int i = 0;i<availableObjects.Count; i++)
        {
            availableObjects[i].GetComponent<Renderer>().enabled = visibility;
            availableObjects[i].GetComponent<LineRenderer>().enabled = visibility;
        }
        for(int i = 0; i<availableArrows.Count; i++)
        {
            availableArrows[i].GetComponent<Renderer>().enabled = visibility;
        }
    }
}
