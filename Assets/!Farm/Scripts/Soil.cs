using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    private Crop crop;
    public bool HasCrop => crop != null;

    public void AddCrop(Crop crop) => this.crop = crop;
}
