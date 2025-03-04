using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soil : MonoBehaviour, IPointerClickHandler
{
    public static bool S_CanHarvest = false;

    private Crop crop;
    public bool HasCrop => crop != null;

    public void AddCrop(Crop crop) => this.crop = crop;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!S_CanHarvest)
            return;

        if (!crop)
            return;

        if(crop.ReadyToHarvest) //Harvest
        {
            Destroy(crop.gameObject);
        }
    }
}
