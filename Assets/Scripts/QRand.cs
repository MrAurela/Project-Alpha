// PRNG with pseudo quantum error
// 2021 © Aaron Campbell


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

    // Multiplier constant for PRNG
    private static readonly long PRNGMultiplier = 1664525;
    // Increment constant for PRNG
    private static readonly long PRNGConstant = 1013904223;
    // Modulus constant for PRNG
    private static readonly double PRNGModulus = System.Math.Pow(2,32);
    // Default quantum register
    private static readonly List<List<double>> OfflineCounts = new List<List<double>>{new List<double>{0.6839729119638827, 0.7697516930022573, 0.8893905191873589, 0.9051918735891648, 0.9232505643340858, 0.9367945823927766, 0.945823927765237, 1.0}, new List<double>{0.022375215146299483, 0.043029259896729774, 0.055077452667814115, 0.11015490533562823, 0.14113597246127366, 0.29259896729776247, 0.35111876075731496, 1.0}};

    //
    // Instance variable definitions
    //

    // Quantum register
    List<List<double>> countsList;
    // Current seed
    double currentSeed;

    //
    // Function definitions
    //

    void Start()
    {
        // Initialize this.currentSeed to CurrentTimeMillis();
        this.currentSeed = CurrentTimeMillis();
        // Initialize this.countsList
        this.countsList = OfflineCounts;
    }

    /// <summary>
    /// Sets the starting seed so that the same QPRNG sequence can be repeated.
    /// </summary>
    /// <param name="startingSeed">Int value which the QRand starting seed will be set to.</param>
    public void InitState(int startingSeed)
    {
        this.currentSeed = startingSeed;
    }

    /// <summary>
    /// Returns the current seed's value as a double.
    /// </summary>
    /// <returns>Returns the current seed's value as a double.</returns>
    public double GetCurrentSeed()
    {
        return (double)this.currentSeed;
    }

    /// <summary>
    /// Returns the current seed's scaled value as a double.
    /// </summary>
    /// <returns>Returns the current seed's scaled value as a double.</returns>
    public double GetCurrentSeedDouble()
    {
        return (double)(this.currentSeed/PRNGModulus);
    }

    /// <summary>
    /// Returns the current seed's scaled value as an int.
    /// </summary>
    /// <returns>Returns the current seed's scaled value as an int.</returns>
    public int GetCurrentSeedInt(int maxValue = 1000)
    {
        return (int)(maxValue*this.currentSeed/PRNGModulus);
    }

    /// <summary>
    /// Returns the current seed's scaled value as a float.
    /// </summary>
    /// <returns>Returns the current seed's scaled value as a float.</returns>
    public float GetCurrentSeedFloat()
    {
        return (float)(this.currentSeed/PRNGModulus);
    }

    /// <summary>
    /// Computes and a new seed using a linear congruent classic PRNG and bitwise pseudo quantum error.
    /// </summary>
    /// <returns>Returns the computed seed without scaling.</returns>
    public double NextSeed()
    {
        // preform linear congruent classic calculation
        double retval = (PRNGMultiplier * this.currentSeed + PRNGConstant) % PRNGModulus;
        // collect the decimals of stuff
        double d = retval%1;
        // preform bitwise pseudo quantum error
        retval = ApplyQuantumError(retval) + d;
        // update currentSeed
        this.currentSeed = retval;
        // return scaled seed
        return retval;
    }

    /// <summary>
    /// Computes and a new seed using a linear congruent classic PRNG and bitwise pseudo quantum error.
    /// </summary>
    /// <returns>Returns the computed seed as a double in the range [0,1).</returns>
    public double NextDouble()
    {
        return this.NextSeed()/PRNGModulus;
    }

    /// <summary>
    /// Computes and a new seed using a linear congruent classic PRNG and bitwise pseudo quantum error.
    /// </summary>
    /// <returns>Returns the computed seed as a float in the range [0,1).</returns>
    public float NextFloat()
    {
        return (float)this.NextDouble();
    }

    /// <summary>
    /// Computes and a new seed using a linear congruent classic PRNG and bitwise pseudo quantum error.
    /// </summary>
    /// <returns>Returns the computed seed as an int in the range [0,maxValue).</returns>
    /// <param name="maxValue">The maximum value of the returned seed. 1000 by default.</param>
    public int NextInt(int maxValue = 1000)
    {
        return (int)(this.NextDouble() * maxValue);
    }

    /// <summary>
    /// Computes and a new seed using a linear congruent classic PRNG and bitwise pseudo quantum error.
    /// </summary>
    /// <returns>Returns the computed seed as an int in the range (minValue,maxValue).</returns>
    /// <param name="maxValue">The maximum value of the returned seed. 1000 by default.</param>
    /// <param name="minValue">The minimum value of the returned seed. -1000 by default.</param>
    public double Range(int minValue = -1000, int maxValue = 1000)
    {
        double maxRand = this.NextDouble() * maxValue;
        double minRand = this.NextDouble()* minValue;

        return (maxRand+minRand)/2;
    }

    /// <summary>
    /// Get the current time in milliseconds since Jan 1, 1970.
    /// </summary>
    /// <returns>Current time in milliseconds since Jan 1, 1970.</returns>
    static long CurrentTimeMillis()
    {
        return (long)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
    }

    /// <summary>
    /// A call for NextRegister_Coroutine() intended for QRand initialization which will print a completion message to the Debug.Log when complete.
    /// </summary>
    public IEnumerator InitQRand()
    {
        // Re-initialize this.currentSeed to CurrentTimeMillis();
        this.currentSeed = CurrentTimeMillis();
        // Re-initialize this.countsList
        this.countsList = OfflineCounts;
        yield return StartCoroutine(NextRegister_Coroutine());
        Debug.Log("QRand fully initialized.");
        // PrintCounts(this.countsList);
    }

    /// <summary>
    /// Get a new quantum register from the quantum-seed-generator server.
    /// </summary>
    IEnumerator NextRegister_Coroutine()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://quantum-seed-generator.herokuapp.com/get-seeds"))
        {
            Debug.Log("Fetching quantum register from quantum-seed-generator.");
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            Debug.Log("Switch Statement");
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("Error fetching counts from server");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Error fetching counts from server");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("Error fetching counts from server");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Request recieved from quantum-seed-generator.");
                    this.countsList = CreateArray(webRequest.downloadHandler.text);
                    break;
                default:
                    Debug.Log("Error fetching counts from server");
                    break;
            }
        }
    }

    /// <summary>
    /// Given an input string representing an Int[], creates a new List<\List<\double>> to be used by bitwise pseudo quantum error.
    /// </summary>
    /// <returns>List<\List<\double>> representing the outcome probabilities of a real quantum circuit.</returns>
    /// <param name="msg">String representing an Int[] array. Either retrieved via webrequest from a real quantum computer or the default message.</param>
    List<List<double>> CreateArray(string counts)
    {
        // Split the list into 2 halves
        List<List<double>> retval = new List<List<double>>(){new List<double>(),new List<double>()};
        string[] numList = counts.Substring(1,counts.Length-3).Split(',');
        int subLength = numList.Length/2;
        for(int i = 0; i < numList.Length; i++)
        {
            retval[(i/subLength)%2].Add(Convert.ToInt64(numList[i]));
        }
        // Reformat counts to desired percentage format
        for(int i = 0; i < 2; i++)
        {
            for(int j = 1; j < subLength; j++)
            {
                retval[i][j] += retval[i][j-1];
            }
            for(int j = 0; j < subLength; j++)
            {
                retval[i][j] /= retval[i][subLength-1];
            }
        }
        return retval;
    }

    /// <summary>
    /// A function to print the value of a List<List<double>> in a human readable form to Debug.Log.
    /// </summary>
    /// <param name="args">The List<List<double>> to be logged.</param>
    void PrintCounts(List<List<double>> args)
    {
        List<string> retval = new List<string>{};
        foreach (List<double> list in args)
        {
            retval.Add(string.Join(",",list));
        }
        Debug.Log("[[" + string.Join("],[",retval) + "]]");
    }

    /// <summary>
    /// Given a double n, preforms bitwise pseudo quantum error on n.
    /// </summary>
    /// <returns>N after being affected by bitwise pseudo quantum error plus the decimal values from the original number after being scaled by PRNGModulus.</returns>
    /// <param name="n">The number to be affected by quantum error.</param>
    double ApplyQuantumError(double num)
    {
        // Isolate decimal of num after scaling with PRNGModulus
        double d = (num/PRNGModulus)%1;
        // Return value
        long retval = 0;
        // Apply quantum bit error to n bitwise
        for (int i = 0; i < 32; i++)
        {
            long bit = 1&((long)num)>>(31-i);
            retval |= QuantumBitError(bit,d)<<i;
        }
        // Return int value of modified bit sequence adding back the lost decimals
        return retval;
    }

    /// <summary>
    /// Applies pseudo quantum error to the given bit.
    /// </summary>
    /// <returns>Char value after being affected by pseudo quantum error.</returns>
    /// <param name="v">Char value to be affected by quantum error.</param>
    /// <param name="d">Decimal value used to determine output of pseudo quantum error.</param>
    long QuantumBitError(long bit, double d)
    {
        int index = 0;
        // Set index based on value of d
        while(d>this.countsList[(int)bit&1][index])
        {
            index++;
        }
        return index&1;
    }
}