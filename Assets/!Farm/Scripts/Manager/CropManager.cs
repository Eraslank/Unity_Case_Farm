using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviourSingleton<CropManager>
{
    List<Crop> crops = new List<Crop>();
    public void RegisterCrop(Crop crop)
    {
        if (!crops.Contains(crop))
            crops.Add(crop);
    }

    public void UnRegisterCrop(Crop crop)
    {
        if (crops.Contains(crop))
            crops.Remove(crop);
    }
}
