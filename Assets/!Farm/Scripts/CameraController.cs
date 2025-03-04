using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Vector3 boundry;

    Vector3 origin;
    Vector3 difference;

    bool drag = false;

    public bool isActive = true;

    private void LateUpdate()
    {
        if (!isActive)
            return;

        if (Input.GetMouseButton(0))
        {
            var worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            difference = worldPoint - transform.position;

            if (!drag)
            {
                drag = true;
                origin = worldPoint;
            }
        }
        else
            drag = false;

        if(drag)
        {
            SetCamPos(origin - difference);
        }
    }

    private void SetCamPos(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, -boundry.x, boundry.x);
        pos.y = Mathf.Clamp(pos.y, -boundry.y, boundry.y);
        pos.z = Mathf.Clamp(pos.z, -boundry.z, boundry.z);
        transform.position = pos;
    }
}
