// PRNG with Pseudo Quantum Error
// 2021 © Aaron Campbellusing System;


//
// Imports
//
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class QRand : MonoBehaviour
{
    //
    // Constant definitions
    //

    // multiplier
    static long A = 1664525;
    // increment
    static long C = 1013904223;
    // modulus
    static double M = System.Math.Pow(2,32);
    // default string for Quantum
    static string DefaultStr = "[228, 1, 8, 16, 1, 29, 3, 2, 45, 2, 26, 5, 51, 4, 47, 16, 318, 31, 1, 28, 1, 30, 2, 25, 3, 7, 4, 35, 3, 24, 28] ";

    //
    // Instance variable definitions
    //

    // quantum register
    double[,] countsList;

    double currentSeed;

    //
    // Function definitions
    //

    void Start()
    {
        this.currentSeed = CurrentTimeMillis();
        Debug.Log(this.currentSeed);
        // this.NextRegister();
        this.NextRegister_Coroutine();
        Debug.Log(this.currentSeed);
        this.NextInt();
        Debug.Log(this.currentSeed);
    }

    void NextRegister() => StartCoroutine(NextRegister_Coroutine());

    IEnumerator NextRegister_Coroutine()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://quantum-seed-generator.herokuapp.com/get-seeds"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    this.countsList = CreateArray(DefaultStr);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    this.countsList = CreateArray(DefaultStr);
                    break;
                case UnityWebRequest.Result.Success:
                    this.countsList = CreateArray(webRequest.downloadHandler.text);
                    break;
            }
            this.NextInt();
        }
    }

    int GetCurrentSeed(int maxNumberOfDigits = 3)
    {
        return ToBoundsFloat(this.currentSeed,maxNumberOfDigits);
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

    // scales given seed to the bounds: 0 to max
    int ToBoundsFloat(double num, int maxNumberOfDigits)
    {
        return Convert.ToInt32(num / M * Convert.ToInt32(new string ('9', maxNumberOfDigits)));
    }

    // creates a double[,] given a double[] as a string
    double[,] CreateArray(string msg)
    {
        List<double> rawCountsList = new List<double>();
        string[] numList = msg.Substring(1,msg.Length-3).Split(',');
        foreach(string s in numList)
        {
            rawCountsList.Add(Convert.ToInt64(s));
        }
        double[] rawCounts = rawCountsList.ToArray();
        double[,] countsList = new double[rawCounts.Length/2,rawCounts.Length/2];
        int subLength = rawCounts.Length/2;
        for(int i = 0; i < rawCounts.Length; i++)
        {
            int index = i/subLength;
            countsList[index,i%subLength] = rawCounts[i];
        }
        // reformat counts to desired percentage format
        for(int i = 0; i < 2; i++)
        {
            for(int j = 1; j < subLength; j++)
            {
                countsList[i,j] += countsList[i,j-1];
            }
            for(int j = 0; j < subLength; j++)
            {
                countsList[i,j] /= countsList[i,subLength-1];
            }
        }
        return countsList;
    }

    // takes 2 parameters:
    //    v: single binary digit as a string
    //    f: float value < 0
    // returns a single binary digit as a string which has been changed based on the cuttoffs in the count list based on the float value
    char QuantumBitError(char v, double f)
    {
        int index = 0;
        int initialValue = 0;
        if (v=='1')
        {
            initialValue = 1;
        }
        // set index based on value of f
        Debug.Log(this.countsList); //this.countList is null here
        while(f>this.countsList[initialValue,index])
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
    double Magic(double n)
    {
        // create string representing binary value of n
        char[] binaryArray = DecimalToBinary(Convert.ToInt64(n)).ToCharArray();
        // isolate decimal of ToBoundsFloat(n)
        double f = ToBoundsFloat(n,3)-Convert.ToInt64(ToBoundsFloat(n,3));
        // apply quantum bit error to n bitwise
        for (long i = 0; i < binaryArray.Length; i++)
        {
            binaryArray[i] = QuantumBitError(binaryArray[i],f);
            f*=100;
            f-=Convert.ToInt64(f);
        }
        // return int value of modified bit sequence adding back the lost decimals
        return BinaryToDecimal(new string(binaryArray)) + n-Convert.ToInt64(n);
    }

    // returns the next seed according to the Linear Congruent Classic PRNG
    int NextInt(int maxNumberOfDigits = 3)
    {
        // preform linear congruent classic calculation
        double ret = (A * this.currentSeed + C) % M;
        // collect the decimals of stuff
        double f = ret-Convert.ToInt64(ret);
        // preform bitwise pseudo quantum error
        ret = Magic(ret) + f;
        // update currentSeed
        this.currentSeed = ret;
        // return scaled seed
        Debug.LogFormat("Seed: {0}",ToBoundsFloat(ret,maxNumberOfDigits));
        return ToBoundsFloat(ret,maxNumberOfDigits);
    }
}