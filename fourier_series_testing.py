import cmath
import time
import turtle
import numpy as np
import math
import tkinter as tk
import msvcrt


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

    # #f(t) is a Lissajous pattern
    # return(complex(4*math.sin(2*2*math.pi*t),3*math.cos(6*2*math.pi*t)))

    #f(t) is a step-spiral input is time, output is a complex number
    return(math.floor(10*t)*cmath.rect(1,7*cmath.pi*t))



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


def draw_function(f,end_time,draw_scale=1,time_scale=1.0,colour="black",animate=1):
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


def perform_remaining():
    custom_func=make_custom_function(points)

    print(custom_func(0.5))
    print(custom_func(0.75))
    print(custom_func(0.25))
    print(custom_func(2))
    print(custom_func(-1))
    print(custom_func(0))
    print(custom_func(1))

    # root = tk.Tk()
    # canvas = turtle.Canvas(root)
    # canvas.pack(expand=True, fill='both')
    # #tess = turtle.RawTurtle(canvas)
    # root.mainloop()


    coefficients=find_fourier_coefficients(custom_func,50,0.001)
    print("Fourier coefficients found")
    print(coefficients)
    fourier_series=find_fourier_series(coefficients)
    print("Got the fourier series")
    print("Drawing original function")
    #draw_function(custom_func,end_time=1,draw_scale=1,time_scale=1/10,animate=0)
    print("Drawing fourier series")
    draw_function(fourier_series,end_time=2,draw_scale=1,time_scale=1/30,colour="red",animate=1)


def print_msg():
    print("Hi")

def register_turtle_movements(points):
    # root = tk.Tk()
    # canvas = turtle.Canvas(root)
    # canvas.pack(expand=True, fill='both')
    # custom_function_turtle = turtle.RawTurtle(canvas)
    custom_function_turtle=turtle.Turtle("circle")
    custom_function_turtle.shapesize(0.2, 0.2, 0)
    custom_function_turtle.penup()

    def next_point(x, y):
        custom_function_turtle.goto(x, y)
        points.append(custom_function_turtle.pos())
        custom_function_turtle.pendown()

    turtle.listen()
    turtle.onscreenclick(next_point)
    turtle.onkey(custom_function_turtle.clear,"Down")
    turtle.onkey(perform_remaining,"Up")

    turtle.done()

def make_custom_function(points):

    points.append(points[0])
    #Making function out of collected points
    print("points is",points)
    total_dis=total_distance(points)
    print("total distance is",total_dis)
    custom_function=get_function(points, total_dis)
    return(custom_function)


def total_distance(points):
    total_dis=0
    prev_point=points[0]
    for point in points[1:]:
        total_dis+=math.sqrt((prev_point[0]-point[0])**2+(prev_point[1]-point[1])**2)
        prev_point=point

    return(total_dis)

def get_function(points,total_dis):
    cur_dis=0
    prev_point=points[0]
    function_reference=[]
    function_reference.append(tuple([cur_dis,prev_point]))
    for i,point in enumerate(points[1:]):
        cur_dis += math.sqrt((prev_point[0] - point[0]) ** 2 + (prev_point[1] - point[1]) ** 2)
        function_reference.append(tuple([cur_dis/total_dis,point]))
        prev_point=point

    print("function_reference is")
    for fr in function_reference:
        print(fr)
    def function_from_points(t):
        if t<0 or t>1:
            print("error t is",t)
            return(0)
        else:
            end=len(function_reference)-1
            start=0
            #Performing binary search to find the appropriate interval
            while(start+1<end):
                mid = (end + start) // 2
                if function_reference[mid][0]>t:
                    end=mid
                else:
                    start=mid

            #print("for t=",t,"interval found =",start)

            #Interpolating in the interval to return final function value
            first_point=function_reference[start][1]
            first_normalised_distance=function_reference[start][0]
            next_point=function_reference[end][1]
            next_normalised_distance=function_reference[end][0]
            x_val=first_point[0]+((t-first_normalised_distance)/(next_normalised_distance-first_normalised_distance))*(next_point[0]-first_point[0])
            y_val=first_point[1]+((t-first_normalised_distance)/(next_normalised_distance-first_normalised_distance))*(next_point[1]-first_point[1])

            return(complex(x_val,y_val))

    return(function_from_points)




points=[]
register_turtle_movements(points)


