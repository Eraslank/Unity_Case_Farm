using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementUIController : MonoBehaviour
{
    [SerializeField] GameObject contextParent;
    [SerializeField] GameObject buildingsContext;
    [SerializeField] GameObject cropsContext;

    [SerializeField] GameObject disablePlacementButton;

    private void Start()
    {
        CloseContextPanel();
    }

    public void OpenContextPanel(int id)
    {
        GameManager.Instance.PlacementSystem.StopPlacement();
        GameManager.Instance.CameraController.isActive = false;
        contextParent.SetActive(true);

        buildingsContext.SetActive(id == 0);
        cropsContext.SetActive(id == 1);

        disablePlacementButton.SetActive(true);
    }

    public void StartPlacement(int id)
    {
        CloseContextPanel();
        GameManager.Instance.PlacementSystem.StartPlacement(id);
        GameManager.Instance.CameraController.isActive = false;
        disablePlacementButton.SetActive(true);
    }

    public void StartRemoving()
    {
        CloseContextPanel();
        GameManager.Instance.PlacementSystem.StartRemoving();
        GameManager.Instance.CameraController.isActive = false;
        disablePlacementButton.SetActive(true);
    }

    public void CloseContextPanel()
    {
        contextParent.SetActive(false);
        disablePlacementButton.SetActive(false);
        GameManager.Instance.PlacementSystem.StopPlacement();
        GameManager.Instance.CameraController.isActive = true;
    }
}
