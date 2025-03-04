using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore.GameSystem.Placement
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] InputManager inputManager;
        [SerializeField] Grid grid;
        [SerializeField] Vector2Int gridSize;
        [SerializeField] ObjectsDatabaseSO database;
        [SerializeField] GameObject gridVisualization;
        [SerializeField] PreviewSystem preview;
        [SerializeField] ObjectPlacer objectPlacer;

        private GridData gridData;
        private GridData cropData;

        private Vector3Int lastDetectedPosition = Vector3Int.zero;

        IBuildingState buildingState;

        public event UnityAction OnStartPlacement;
        public event UnityAction OnStartRemoving;
        public event UnityAction OnStopPlacement;

        private void Start()
        {
            gridVisualization.SetActive(false);
            gridData = new(gridSize);
            cropData = new(gridSize);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartPlacement(0);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                StartPlacement(1);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                StartPlacement(2);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                StartPlacement(3);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                StartPlacement(4);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                StartPlacement(5);
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
            buildingState = new PlacementState(ID, grid, preview, database, gridData, cropData, objectPlacer);
            inputManager.OnClicked += OnClick;
            inputManager.OnExit += StopPlacement;

            OnStartPlacement?.Invoke();
        }

        public void StartRemoving()
        {
            StopPlacement();
            gridVisualization.SetActive(true);
            buildingState = new RemovingState(grid, preview, gridData, cropData, objectPlacer);
            inputManager.OnClicked += OnClick;
            inputManager.OnExit += StopPlacement;

            OnStartRemoving?.Invoke();
        }

        private void OnClick()
        {
            StartCoroutine(C_OnClick());
            IEnumerator C_OnClick()
            {
                //Wait For Single Frame To Allow UI to Compute First.
                yield return new WaitForEndOfFrame();

                if (inputManager.IsPointerOverUI())
                    yield break;

                buildingState.OnClick(lastDetectedPosition);
            }

        }

        public void StopPlacement()
        {
            if (buildingState == null)
                return;
            gridVisualization.SetActive(false);
            buildingState.EndState();
            inputManager.OnClicked -= OnClick;
            inputManager.OnExit -= StopPlacement;
            lastDetectedPosition = Vector3Int.zero;
            buildingState = null;

            OnStopPlacement?.Invoke();
        }
    }
}