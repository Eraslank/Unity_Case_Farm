using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] Transform meshParent;
    [SerializeField] GameObject[] stages;

    [SerializeField] Vector3 maxGrowth = Vector3.one;
}
