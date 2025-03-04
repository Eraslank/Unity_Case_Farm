using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlaceableType
{
    NONE = -1,
    Default,

    Building,
    Crop,
}

public static class ColorExtension
{
    public static Color WithA(this Color c, float a)
    {
        c.a = a;
        return c;
    }
}