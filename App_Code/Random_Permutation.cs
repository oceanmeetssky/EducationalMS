using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Random_Permutation
/// </summary>
public class Random_Permutation
{
    private int[] num;

    private void Swap<T>(ref T a, ref T b)
    {
        T t;
        t = a;
        a = b;
        b = t;
    }

    public Random_Permutation(int n)
    {
        num = new int[n];
        for (int t = 0; t < n; t++)
        {
            num[t] = t;
        }

    }

    private void Next()
    {
        Random GetRandNumber = new Random();

        for (int t = 0; t < num.Length * 5; t++)
        {
            //0 ~ n-1
            int i = GetRandNumber.Next(0, num.Length);
            int j = (i + GetRandNumber.Next(1, num.Length)) % num.Length;
            Swap<int>(ref num[i], ref num[j]);
        }
    }

    public int[] NextRandom()
    {
        Next();
        return num;
    }
}
