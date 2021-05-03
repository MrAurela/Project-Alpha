// PRNG with Pseudo Quantum Error
// 2021 © Aaron Campbell

// How to use:
//   1. Set the variables under ""Global var definitions" to the desired values.
//   2. Run the program
//   3. Note that for the program to re-run the same way, you must record the value of counts and re-use it rather than getting a new one

//
// Imports
//

using System;
using UnityEngine;
using UnityEngine.UI;

public class QPRNG : MonoBehaviour {

    //
    // Constant definitions
    //

    // multiplier
    static long A = 1664525;

    // increment
    static long C = 1013904223;

    // modulus
    static double M = System.Math.Pow(2,32);

    //
    // Function definitions
    //

    // pulls latest counts from server
    static double[,] GetCounts()
    {
        // some shit here to get from server
        double[,] countsList = {{0.6839729119638827, 0.7697516930022573, 0.8893905191873589, 0.9051918735891648, 0.9232505643340858, 0.9367945823927766, 0.945823927765237, 1.0}, {0.022375215146299483, 0.043029259896729774, 0.055077452667814115, 0.11015490533562823, 0.14113597246127366, 0.29259896729776247, 0.35111876075731496, 1.0}};
        return countsList;
    }

    // Gets the current time in milliseconds since Jan 1, 1970
    static long CurrentTimeMillis()
    {
        return (long) (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
    }

    // convert a base 10 long to its binary representation as a string
    static string DecimalToBinary(long n)
    {
        return Convert.ToString(n, 2);;
    }

    // convert a binary string longo its base 10 representation as an long
    static long BinaryToDecimal(string n)
    {
        return Convert.ToInt64(n,2);
    }

    // takes 2 parameters:
    //    v: single binary digit as a string
    //    f: float value < 0
    // returns a single binary digit as a string which has been changed based on the cuttoffs in the count list based on the float value
    static char QuantumBitError(char v, double f, double[,] countsList)
    {
        int index = 0;
        int initialValue = 0;
        if (v=='1')
        {
            initialValue = 1;
        }
        // set index based on value of f
        while(f>countsList[initialValue,index])
        {
            index++;
        }
        if(index%2 == 1)
        {
            return '1';
        } else
        {
            return '0';
        }
    }

    // given a float n, returns the double with the part of the value > 0 affected by the quantum circuit
    static double Magic(double n, int max, double[,] countsList)
    {
        // create string representing binary value of n
        char[] binaryArray = DecimalToBinary(Convert.ToInt64(n)).ToCharArray();
        // isolate decimal of ToBoundsFloat(n)
        double f = ToBoundsFloat(n,max)-Convert.ToInt64(ToBoundsFloat(n,max));
        // apply quantum bit error to n bitwise
        for (long i = 0; i < binaryArray.Length; i++)
        {
            binaryArray[i] = QuantumBitError(binaryArray[i],f,countsList);
            f*=100;
            f-=Convert.ToInt64(f);
        }
        // return int value of modified bit sequence adding back the lost decimals
        return BinaryToDecimal(new string(binaryArray)) + n-Convert.ToInt64(n);
    }

    // returns the next seed according to the Linear Congruent Classic PRNG
    static double NewSeed(double seed, int max, double[,] countsList)
    {
        // preform linear congruent classic calculation
        double ret = (A * seed + C) % M;
        // collect the decimals of stuff
        double f = ret-Convert.ToInt64(ret);
        // preform bitwise pseudo quantum error
        ret = Magic(ret,max,countsList) + f;
        return ret;
    }

    // scales given seed to the bounds: 0 to max
    static double ToBoundsFloat(double seed, int max)
    {
        return seed / M * max;
    }

    //
    // Seed generation
    //

    public static long[] GenerateSeeds(int numberOfSeeds = 20, int maxNumberOfDigits = 3)
    {
        long[] retval = new long[numberOfSeeds];
        // initialize the starting value to least significant digits of CurrentTimeMillis
        double seed = CurrentTimeMillis();
        // set max seed value
        int max = Convert.ToInt32(new string ('9', maxNumberOfDigits));
        // set counts list
        double[,] countsList = GetCounts();

        // create seeds
        Debug.Log("Generating Seeds:");
        for (long i = 0; i < numberOfSeeds; i++)
        {
            seed = NewSeed(seed, max, countsList);
            Debug.LogFormat("  - seed[{0}]:\t{1}",i,Convert.ToInt64(ToBoundsFloat(seed,max)));
            retval[i] = Convert.ToInt64(ToBoundsFloat(seed,max));
        }
        return retval;
    }
}