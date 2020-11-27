using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complex
{
    public double Real;
    public double Imaginary;

    public Complex(double R,double I)
    {
        Real=R;
        Imaginary=I;
    }
    public Complex() :this(0.0d,0.0d) {}

    public override string ToString()
    {
      return "( "+Real+" + i "+Imaginary+" )";
    }

    public static Complex operator+(Complex c1,Complex c2)
    {
        Complex c=new Complex();
        c.Real=c1.Real+c2.Real;
        c.Imaginary=c1.Imaginary+c2.Imaginary;

        return(c);
    }

    public static Complex operator-(Complex c1,Complex c2)
    {
        Complex c=new Complex();
        c.Real=c1.Real-c2.Real;
        c.Imaginary=c1.Imaginary-c2.Imaginary;

        return(c);
    }

    public static Complex operator*(Complex c1,Complex c2)
    {
        Complex c=new Complex();
        c.Real=(c1.Real*c2.Real)-(c1.Imaginary*c2.Imaginary);
        c.Imaginary=(c1.Real*c2.Imaginary)+(c1.Imaginary*c2.Real);

        return(c);
    }

    public static Complex operator*(Complex c1,double d)
    {
        Complex c=new Complex();
        c.Real=(c1.Real*d);
        c.Imaginary=c1.Imaginary*d;

        return(c);
    }

    public static Complex operator/(Complex c1,double d)
    {
        Complex c=new Complex();
        c.Real=(c1.Real/d);
        c.Imaginary=c1.Imaginary/d;

        return(c);
    }

    public static explicit operator Complex(double d) => new Complex(d,0);

    public static Complex FromPolarCoordinates(double mag,double angle)
    {
        Complex c=new Complex();
        c.Real=mag*Math.Cos(angle);
        c.Imaginary=mag*Math.Sin(angle);

        return(c);
    }

    

}