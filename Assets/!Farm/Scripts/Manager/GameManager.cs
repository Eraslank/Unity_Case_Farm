using GameCore.GameSystem.Placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] PlacementSystem placementSystem;
    [SerializeField] CameraController cameraController;
    [SerializeField] PhysicsRaycaster physicsRaycaster;

    public PlacementSystem @PlacementSystem => placementSystem;
    public CameraController @CameraController => cameraController;
    private void Start()
    {
        Soil.S_CanHarvest = false;
    }

    private void OnEnable()
    {
        placementSystem.OnStartPlacement += OnStartPlacement;
        placementSystem.OnStartRemoving += OnStartRemoving;

        placementSystem.OnStopPlacement += OnStopPlacement;
    }
    private void OnDisable()
    {
        placementSystem.OnStartPlacement -= OnStartPlacement;
        placementSystem.OnStartRemoving -= OnStartRemoving;

        placementSystem.OnStopPlacement -= OnStopPlacement;
    }

    private void OnStopPlacement()
    {
        Soil.S_CanHarvest = true;
        physicsRaycaster.enabled = true;
    }

    private void OnStartRemoving()
    {
        Soil.S_CanHarvest = false;
        physicsRaycaster.enabled = false;
    }

    private void OnStartPlacement()
    {
        Soil.S_CanHarvest = false;
        physicsRaycaster.enabled = false;
    }
}
