import cmath
import time
import turtle
import numpy as np
import math

c1=cmath.rect(1,2*cmath.pi*3.3)
print(c1)
print(3*c1)

#Making a complex function to find its fourier series
def f(t):
    # #f(t) is a spiral input is time, output is a complex number
    # return(10*t*cmath.rect(1,7*cmath.pi*t))

    # #f(t) is an oval
    # return(complex(4*cmath.cos(2*cmath.pi*t),3*cmath.sin(2*cmath.pi*t)))

    # #f(t) is a flower-shape
    # return(cmath.rect(4*cmath.sin(4*cmath.pi*t).real,2*cmath.pi*t))

    #f(t) is a Lissajous pattern
    return(complex(4*math.sin(2*2*math.pi*t),3*math.cos(6*2*math.pi*t)))


def find_fourier_coefficients(function, max_frequency, delta_for_integration):
    '''
    :param function: Function whose Fourier coefficients is required
    :param max_frequency: Maximum frequency in the series
    :param delta_for_integration: Between 0 and 1, used to perform "integration"
    :return: A dictionary of coefficients. Range is [-max_frequency,max_frequency]
    '''
    coefficients={}
    for frequency in range(-max_frequency,max_frequency+1):
        coefficients[frequency]=0
        ck=0
        #Performing integration
        for t in np.arange(0,1,delta_for_integration):
            ck+=cmath.rect(1,-2*cmath.pi*frequency*t)*function(t)*delta_for_integration

        coefficients[frequency]=ck

    return(coefficients)


def find_fourier_series(coefficients):
    '''
    This function returns a function that is the fourier series corresponding to the fourier coefficients
    :param coefficients: dictionary of coefficients with keys as frequencies
    :return: function that is the fourier series corresponding to the fourier coefficients
    '''
    # making the function
    def fourier_series(t):
        return_val=0
        for freq in coefficients:
            return_val+=coefficients[freq]*cmath.rect(1,2*cmath.pi*freq*t)

        return(return_val)

    return(fourier_series)


def draw_function(f,end_time,draw_scale=1,time_scale=1,colour="black",animate=1):
    # sc=turtle.Screen()
    # if(not animate):
    #     sc.tracer(False)

    function_drawer=turtle.Turtle("circle")
    function_drawer.shapesize(0.1,0.1,0)
    function_drawer.color(colour,colour)
    function_drawer.penup()
    pos0 = f(0) * draw_scale
    function_drawer.goto(pos0.real, pos0.imag)
    function_drawer.pendown()

    start=time.time()

    while((time.time()-start)*time_scale<end_time):
        newpos=f((time.time()-start)*time_scale)*draw_scale
        function_drawer.goto(newpos.real,newpos.imag)

    # if(not animate):
    #     sc.update()
    #     sc.tracer(True)


coefficients=find_fourier_coefficients(f,10,0.001)
print("Fourier coefficients found")
print(coefficients)
fourier_series=find_fourier_series(coefficients)
print("Got the fourier series")
print("Drawing original function")
draw_function(f,1,50,1/20,animate=0)
print("Drawing fourier series")
draw_function(fourier_series,20,50,1/20,colour="red",animate=1)
