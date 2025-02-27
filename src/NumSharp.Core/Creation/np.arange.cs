﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;

namespace NumSharp
{
    public static partial class np
    {
        /// <summary>
        /// Return evenly spaced values within a given interval.
        /// 
        /// Values are generated within the half-open interval [start, stop)
        /// (in other words, the interval including start but excluding stop).
        /// For integer arguments the function is equivalent to the Python built-in
        /// range function, but returns an ndarray rather than a list.
        /// 
        /// When using a non-integer step, such as 0.1, the results will often not
        /// be consistent.  It is better to use numpy.linspace for these cases.
        /// </summary>
        /// <param name="stop">
        /// End of interval.  The interval does not include this value, except
        /// in some cases where step is not an integer and floating point
        /// round-off affects the length of out.
        /// </param>
        /// <returns>
        /// Array of evenly spaced values.
        /// 
        /// For floating point arguments, the length of the result is
        /// ceil((stop - start)/step).  Because of floating point overflow,
        /// this rule may result in the last element of out being greater
        /// than stop.
        /// </returns>
        public static NDArray arange(float stop)
        {
            return arange(0, stop, 1);
        }

        /// <summary>
        /// Return evenly spaced values within a given interval.
        /// 
        /// Values are generated within the half-open interval [start, stop)
        /// (in other words, the interval including start but excluding stop).
        /// For integer arguments the function is equivalent to the Python built-in
        /// range function, but returns an ndarray rather than a list.
        /// 
        /// When using a non-integer step, such as 0.1, the results will often not
        /// be consistent.  It is better to use numpy.linspace for these cases.
        /// </summary>
        /// <param name="stop">
        /// End of interval.  The interval does not include this value, except
        /// in some cases where step is not an integer and floating point
        /// round-off affects the length of out.
        /// </param>
        /// <returns>
        /// Array of evenly spaced values.
        /// 
        /// For floating point arguments, the length of the result is
        /// ceil((stop - start)/step).  Because of floating point overflow,
        /// this rule may result in the last element of out being greater
        /// than stop.
        /// </returns>
        public static NDArray arange(double stop)
        {
            return arange(0, stop, 1);
        }

        /// <summary>
        /// Return evenly spaced values within a given interval.
        /// 
        /// Values are generated within the half-open interval [start, stop)
        /// (in other words, the interval including start but excluding stop).
        /// For integer arguments the function is equivalent to the Python built-in
        /// range function, but returns an ndarray rather than a list.
        /// 
        /// When using a non-integer step, such as 0.1, the results will often not
        /// be consistent.  It is better to use numpy.linspace for these cases.
        /// </summary>
        /// <param name="start">
        /// Start of interval.  The interval includes this value.  The default
        /// start value is 0.
        /// </param>
        /// <param name="stop">
        /// End of interval.  The interval does not include this value, except
        /// in some cases where step is not an integer and floating point
        /// round-off affects the length of out.
        /// </param>
        /// <param name="step">
        /// Spacing between values.  For any output out, this is the distance
        /// between two adjacent values, out[i+1] - out[i].  The default
        /// step size is 1.  If step is specified as a position argument,
        /// start must also be given.
        /// </param>
        /// <returns>
        /// Array of evenly spaced values.
        /// 
        /// For floating point arguments, the length of the result is
        /// ceil((stop - start)/step).  Because of floating point overflow,
        /// this rule may result in the last element of out being greater
        /// than stop.
        /// </returns>
        public static NDArray arange(float start, float stop, float step = 1)
        {
            if (start > stop)
            {
                throw new Exception("parameters invalid, start is greater than stop.");
            }

            int length = (int)Math.Ceiling((stop - start + 0.0) / step);

            var nd = new NDArray(typeof(float), new Shape(length));

            float[] puffer = (float[])nd.Array;

            for (int index = 0; index < length; index++)
            {
                float value = start + index * step;
                puffer[index] = value;
            }

            return nd;
        }

        /// <summary>
        /// Return evenly spaced values within a given interval.
        /// 
        /// Values are generated within the half-open interval [start, stop)
        /// (in other words, the interval including start but excluding stop).
        /// For integer arguments the function is equivalent to the Python built-in
        /// range function, but returns an ndarray rather than a list.
        /// 
        /// When using a non-integer step, such as 0.1, the results will often not
        /// be consistent.  It is better to use numpy.linspace for these cases.
        /// </summary>
        /// <param name="start">
        /// Start of interval.  The interval includes this value.  The default
        /// start value is 0.
        /// </param>
        /// <param name="stop">
        /// End of interval.  The interval does not include this value, except
        /// in some cases where step is not an integer and floating point
        /// round-off affects the length of out.
        /// </param>
        /// <param name="step">
        /// Spacing between values.  For any output out, this is the distance
        /// between two adjacent values, out[i+1] - out[i].  The default
        /// step size is 1.  If step is specified as a position argument,
        /// start must also be given.
        /// </param>
        /// <returns>
        /// Array of evenly spaced values.
        /// 
        /// For floating point arguments, the length of the result is
        /// ceil((stop - start)/step).  Because of floating point overflow,
        /// this rule may result in the last element of out being greater
        /// than stop.
        /// </returns>
        public static NDArray arange(double start, double stop, double step = 1)
        {
            if (start > stop)
            {
                throw new Exception("parameters invalid, start is greater than stop.");
            }

            int length = (int)Math.Ceiling((stop - start + 0.0) / step);

            var nd = new NDArray(typeof(double), new Shape(length));

            double[] puffer = (double[])nd.Array;

            for (int index = 0; index < length; index++)
            {
                double value = start + index * step;
                puffer[index] = value;
            }

            return nd;
        }

        /// <summary>
        /// Return evenly spaced values within a given interval.
        /// 
        /// Values are generated within the half-open interval [start, stop)
        /// (in other words, the interval including start but excluding stop).
        /// For integer arguments the function is equivalent to the Python built-in
        /// range function, but returns an ndarray rather than a list.
        /// 
        /// When using a non-integer step, such as 0.1, the results will often not
        /// be consistent.  It is better to use numpy.linspace for these cases.
        /// </summary>
        /// <param name="stop">
        /// End of interval.  The interval does not include this value, except
        /// in some cases where step is not an integer and floating point
        /// round-off affects the length of out.
        /// </param>
        /// <returns>
        /// Array of evenly spaced values.
        /// 
        /// For floating point arguments, the length of the result is
        /// ceil((stop - start)/step).  Because of floating point overflow,
        /// this rule may result in the last element of out being greater
        /// than stop.
        /// </returns>
        public static NDArray arange(int stop)
        {
            return arange(0, stop, 1);
        }

        /// <summary>
        /// Return evenly spaced values within a given interval.
        /// 
        /// Values are generated within the half-open interval [start, stop)
        /// (in other words, the interval including start but excluding stop).
        /// For integer arguments the function is equivalent to the Python built-in
        /// range function, but returns an ndarray rather than a list.
        /// 
        /// When using a non-integer step, such as 0.1, the results will often not
        /// be consistent.  It is better to use numpy.linspace for these cases.
        /// </summary>
        /// <param name="start">
        /// Start of interval.  The interval includes this value.  The default
        /// start value is 0.
        /// </param>
        /// <param name="stop">
        /// End of interval.  The interval does not include this value, except
        /// in some cases where step is not an integer and floating point
        /// round-off affects the length of out.
        /// </param>
        /// <param name="step">
        /// Spacing between values.  For any output out, this is the distance
        /// between two adjacent values, out[i+1] - out[i].  The default
        /// step size is 1.  If step is specified as a position argument,
        /// start must also be given.
        /// </param>
        /// <returns>
        /// Array of evenly spaced values.
        /// 
        /// For floating point arguments, the length of the result is
        /// ceil((stop - start)/step).  Because of floating point overflow,
        /// this rule may result in the last element of out being greater
        /// than stop.
        /// </returns>
        public static NDArray arange(int start, int stop, int step = 1)
        {
            if (start > stop)
            {
                throw new Exception("parameters invalid, start is greater than stop.");
            }

            int length = (int)Math.Ceiling((stop - start + 0.0) / step);
            int index = 0;

            var nd = new NDArray(np.int32, new Shape(length));

            var a = new int[length];
            for (int i = start; i < stop; i += step)
                a[index++] = i;

            nd.ReplaceData(a);

            return nd;
        }
    }
}
