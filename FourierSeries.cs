using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class FourierSeries
{
    private static double pi=3.14159265;
    public static Dictionary<int, Complex>[] GetFourierCoefficients(Func<double,double[]> func,int max_frequency,double time_delta)
    {
        /*
        Uses definition to compute and return the fourier coefficients of func. 
        (Naive implementation)
        */
        Dictionary<int,Complex> coefficientsx=new Dictionary<int,Complex>();
        Dictionary<int,Complex> coefficientsy=new Dictionary<int,Complex>();
        Dictionary<int,Complex> coefficientsz=new Dictionary<int,Complex>();

        for(int i=-1*max_frequency;i<=max_frequency;i++)
        {
            Complex ckx=new Complex(0,0);
            Complex cky=new Complex(0,0);
            Complex ckz=new Complex(0,0);

            for(double t=0;t<1;t+=time_delta)
            {
                Complex multiplier=Complex.FromPolarCoordinates(1.0d,-2*pi*i*t);
                double[] function_value=func(t);

                ckx += multiplier*(function_value[0])*time_delta;
                cky += multiplier*(function_value[1])*time_delta;
                ckz += multiplier*(function_value[2])*time_delta;
            }

            coefficientsx.Add(i,ckx);
            coefficientsy.Add(i,cky);
            coefficientsz.Add(i,ckz);
        }

        Dictionary<int,Complex>[] arr={coefficientsx,coefficientsy,coefficientsz};
        return(arr);
        
    }


    public static Func<double,double[]> GetFourierSeries(Dictionary<int,Complex>[] coefficients)
    {
        /*
        Returns a function func that is the fourier series corresponding to the given coefficients.
        */
        double[] func(double time)
        {
            /*
            f is the function value at time 'time'. 
            It is calculated using the values of the coefficients.
            */
            double[] f=new double[coefficients.Length];
            int i=0;

            foreach(Dictionary<int,Complex> dk in coefficients)
            {
                Complex func_val=new Complex(0,0);
                // Calculating function value of dimension dx at time 'time'
                foreach(KeyValuePair<int,Complex> c in dk)
                {
                    // Integer is frequency, and Complex number is corresponding fourier coefficient
                    // Add partial sum to func_val. This is one term of the series for one dimension
                    Complex multiplier=Complex.FromPolarCoordinates(1.0d,2*pi*c.Key*time);
                    Complex term=multiplier*c.Value*2;
                    func_val=func_val+term;
                }

                f[i]=func_val.Real;
                i+=1;
            }
            return(f);
        }

        return(func);
    }

    public static Dictionary<int, Complex>[] fft_GetFourierCoefficients(Func<double,double[]> func,int max_frequency)
    {
        /* 
        Finds fourier coefficients using Fast Fourier Transform
        max_frequency determines number of samples taken and time diff between each sample
        max_frequency should be a power of 2 (assumed max_frequency<1e7 or ~2^23)
        */
        double time_delta=(1.0d)/max_frequency; 
        int num_of_samples=max_frequency;

        Complex[] function_samples_x=new Complex[num_of_samples];
        Complex[] function_samples_y=new Complex[num_of_samples];
        Complex[] function_samples_z=new Complex[num_of_samples];

        for(int i=0;i<num_of_samples;i++)
        {
            double[] func_val=func(i*time_delta);

            function_samples_x[i]=(Complex)(func_val[0]);
            function_samples_y[i]=(Complex)(func_val[1]);
            function_samples_z[i]=(Complex)(func_val[2]);
        }

        //Applying fft to the three components of the function

        fft_coefficients_main(function_samples_x,num_of_samples,false);
        fft_coefficients_main(function_samples_y,num_of_samples,false);
        fft_coefficients_main(function_samples_z,num_of_samples,false);

        //Building the actual coefficients from the ones we got

        Dictionary<int,Complex> coefficientsx=new Dictionary<int,Complex>();
        Dictionary<int,Complex> coefficientsy=new Dictionary<int,Complex>();
        Dictionary<int,Complex> coefficientsz=new Dictionary<int,Complex>();

        for(int i=0;i<num_of_samples;i++)
        {
            if(i==num_of_samples/2)
                continue;
            int j=i;
            if(i>num_of_samples/2)
                j=(i-(num_of_samples));
            coefficientsx.Add(j,function_samples_x[i]*time_delta/2);
            coefficientsy.Add(j,function_samples_y[i]*time_delta/2);
            coefficientsz.Add(j,function_samples_z[i]*time_delta/2);
        }
        //Fixing for 0
        coefficientsx[0]+=function_samples_x[num_of_samples/2]*time_delta/2;
        coefficientsy[0]+=function_samples_y[num_of_samples/2]*time_delta/2;
        coefficientsz[0]+=function_samples_z[num_of_samples/2]*time_delta/2;

        Dictionary<int,Complex>[] arr={coefficientsx,coefficientsy,coefficientsz};
        return(arr);
        

    }
    public static void fft_coefficients_main(Complex[] function_values,int two_n,bool invert)
    {
        /* Function that implements fft given function samples. 
        It changes the samples to coefficients in place.
        
        n must be a power of 2. minimum n is 1(2^0) which is the base case
          Formula is f_hat=F_2n*f=  |I_n  D_n| * |F_n  0| * |f_even|
                                    |I_n -D_n|   |0  F_n|   |f_odd |

                                =   |I_n  D_n| * |F_n*f_even |  Second part is answer of 2recursive calls
                                    |I_n -D_n|   |F_n*f_odd  |
        */

        //Base case two_n=1
        if(two_n==1)
        {
            return;
        }

        //Case for recursion
        int n=two_n/2;
        Complex[] f_even=new Complex[n];
        Complex[] f_odd= new Complex[n];

        for(int i=0;i<two_n;i+=2)
        {
            f_even[i/2]=function_values[i];
            f_odd[i/2]=function_values[i+1];
        }

        //Performing the recursive step (Divide step)

        fft_coefficients_main(f_even,n,invert);
        fft_coefficients_main(f_odd,n,invert);

        //Computing answer using the answers of the recursive steps (Conquer step)

        Complex omega_2n_power_i=Complex.FromPolarCoordinates(1.0d,0);
        Complex w2n=Complex.FromPolarCoordinates(1.0d,(2*pi*(invert?-1:1))/two_n);
        
        //Performing the matrix multiplication(sparse matrices)

        for(int i=0;i<n;i++)
        {
            
            Complex changed_odd=f_odd[i]*omega_2n_power_i;

            function_values[i]=f_even[i]+changed_odd;
            function_values[i+n]=f_even[i]-changed_odd;

            if(invert)
            {
                function_values[i]/=2;
                function_values[i+n]/=2;
            }
            omega_2n_power_i*=w2n;
        }
    }
}

