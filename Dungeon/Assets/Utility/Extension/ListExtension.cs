using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static bool CheckIndex<T>(this List<T> list, int num)
    {
        return num >= 0 && num < list.Count;
    }
}

