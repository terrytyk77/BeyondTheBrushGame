using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumAbv
{
    public static string prettyValues(int number)
    {
        string formattedNum = "";

        if (number >= 1000000)
        {
            formattedNum = decimal.Round((decimal)((float)number / 1000000f), 2).ToString() + "m";
        }
        else if (number >= 1000)
        {
            formattedNum = decimal.Round((decimal)((float)number / 1000f), 2).ToString() + "k";
        }
        else
        {
            formattedNum = number.ToString();
        }

        return formattedNum;
    }
}
