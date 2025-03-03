using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCore.GameSystem.Placement
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] InputManager inputManager;
        [SerializeField] Grid grid;
        [SerializeField] ObjectsDatabaseSO database;
        [SerializeField] GameObject gridVisualization;
        [SerializeField] PreviewSystem preview;
        [SerializeField] ObjectPlacer objectPlacer;
        [SerializeField] SoundFeedback soundFeedback;

        private GridData gridData;

        private Vector3Int lastDetectedPosition = Vector3Int.zero;

        IBuildingState buildingState;

        private void Start()
        {
            gridVisualization.SetActive(false);
            gridData = new();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartPlacement(0);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                StartPlacement(1);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                StartPlacement(2);
            if (Input.GetKeyDown(KeyCode.X))
                StartRemoving();


            if (buildingState == null)
                return;

            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            if (lastDetectedPosition != gridPosition)
            {
                buildingState.UpdateState(gridPosition);
                lastDetectedPosition = gridPosition;
            }

        }

        public void StartPlacement(int ID)
        {
            StopPlacement();
            gridVisualization.SetActive(true);
            buildingState = new PlacementState(ID, grid, preview, database, gridData, objectPlacer, soundFeedback);
            inputManager.OnClicked += OnClick;
            inputManager.OnExit += StopPlacement;
        }

        public void StartRemoving()
        {
            StopPlacement();
            gridVisualization.SetActive(true);
            buildingState = new RemovingState(grid, preview, gridData, objectPlacer, soundFeedback);
            inputManager.OnClicked += OnClick;
            inputManager.OnExit += StopPlacement;
        }

        private void OnClick()
        {
            if (inputManager.IsPointerOverUI())
                return;

            buildingState.OnClick(lastDetectedPosition);
        }

        private void StopPlacement()
        {
            soundFeedback.PlaySound(SoundType.Click);
            if (buildingState == null)
                return;
            gridVisualization.SetActive(false);
            buildingState.EndState();
            inputManager.OnClicked -= OnClick;
            inputManager.OnExit -= StopPlacement;
            lastDetectedPosition = Vector3Int.zero;
            buildingState = null;
        }
    }
}