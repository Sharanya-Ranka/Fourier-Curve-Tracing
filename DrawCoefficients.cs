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
    //private Func<double,double[]> fourier_series;

    //public LineRenderer line;

    double time=0.0d;
    private static double pi=3.14159265;

    private Vector3 temp_position;
    // Start is called before the first frame update
    public void CalculateFourierSeries(Func<double,double[]> function_to_transform, int max_frequency)
    {
        temp_position=transform.position;

        coefficients=FourierSeries.fft_GetFourierCoefficients(function_to_transform,max_frequency);
        

        for(int i=0;i<(coefficients[0].Count+1)/2;i++)
        {
            availableObjects.Add(Instantiate(Prefab,new Vector3(0, 0, 0), Quaternion.identity));
            availableArrows.Add(Instantiate(ArrowPrefab, new Vector3(0, 0, 0), Quaternion.identity));
        }

        foreach(Dictionary<int,Complex> dk in coefficients)
        {
            //Print all the coefficients for each dimension
            Debug.Log("For dimension");
            foreach(KeyValuePair<int,Complex> c in dk)
            {
                Debug.Log(c.Key+": "+c.Value.Real+"+ i"+c.Value.Imaginary);
            }
            Debug.Log("Hi\n");
        }

        // line=GetComponent<LineRenderer>();
        // line.positionCount=0;
        //line.width=0.04f;
        // Debug.Log(coefficients);
    }

    // Update is called once per frame
    public void AUpdate()
    {
        time+=0.001;
        // double[] pos=fourier_series(time);
        // Debug.Log(pos[0]+" "+pos[1]+" "+pos[2]);
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

                
                Debug.Log("Finding t : Dimension "+dim+" t is "+t);
                pos[dim] = (float)t.Real;
            }
            
            //pos[0]+= 20*go;
            availableArrows[go].transform.position = prev_pos;
            float scaleby_float = Vector3.Distance(prev_pos, prev_pos+pos);
            Vector3 scaleby = new Vector3(scaleby_float, scaleby_float, scaleby_float);
            availableArrows[go].transform.localScale = scaleby;
            availableArrows[go].transform.LookAt(prev_pos+pos);
            if(go == component_count-1)
                availableObjects[go].transform.position= prev_pos+pos;
            prev_pos +=pos;

            
            if(go == component_count-1)
                UpdateLineRenderer(go, prev_pos);
        }

        // temp_position.x=(float)pos[0];
        // temp_position.y=(float)pos[1];
        // temp_position.z=(float)pos[2];

        
        // transform.position=temp_position;

        // UpdateLineRenderer();
        
    }

    public void UpdateLineRenderer(int game_obj_ind, Vector3 pos)
    {
        LineRenderer line = availableObjects[game_obj_ind].GetComponent<LineRenderer>();
        if(time<=1.01)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount-1,pos);
        }
    }
}
