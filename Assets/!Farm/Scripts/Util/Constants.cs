using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtension
{
    public static Color WithA(this Color c, float a)
    {
        c.a = a;
        return c;
    }
}