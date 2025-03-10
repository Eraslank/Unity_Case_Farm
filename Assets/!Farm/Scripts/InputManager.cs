using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

    private Vector3 lastPosition;


    [SerializeField] private LayerMask placementLayermask;

    public event UnityAction OnClicked, OnExit;

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
            OnClicked?.Invoke();
        if(Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out var hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
