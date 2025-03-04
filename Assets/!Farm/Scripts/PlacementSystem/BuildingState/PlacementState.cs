using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameSystem.Placement
{
    public class PlacementState : BuildingState
    {
        private int selectedObjectIndex = -1;
        int ID;
        ObjectsDatabaseSO database;
        EPlaceableType placeableType;

        public PlacementState(int iD,
                              Grid grid,
                              PreviewSystem previewSystem,
                              ObjectsDatabaseSO database,
                              GridData gridData,
                              GridData cropData,
                              ObjectPlacer objectPlacer,
                              SoundFeedback soundFeedback) : base(grid, previewSystem, gridData, cropData, objectPlacer, soundFeedback)
        {
            ID = iD;
            this.database = database;

            selectedObjectIndex = database.data.FindIndex(data => data.id == ID);
            if (selectedObjectIndex > -1)
            {
                placeableType = database.data[selectedObjectIndex].placeableType;
                previewSystem.StartShowingPlacementPreview(
                    database.data[selectedObjectIndex].prefab,
                    database.data[selectedObjectIndex].size);
            }
            else
                throw new System.Exception($"No object with ID {iD}");

        }
        protected override bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size)
        {
            var baseResult = base.CanPlaceObjectAt(gridPosition, size);
            if (placeableType == EPlaceableType.Building)
                return baseResult;
            else //If Placing Crop
            {
                if (baseResult) //Has No Building (Inc. Soil)
                    return false;

                var placedObject = objectPlacer.GetObjectAt(gridData.GetGameObjectIndex(gridPosition));

                if (!placedObject.TryGetComponent<Soil>(out var soil) || soil.HasCrop) //Placed Object Is Not Soil || Soil has crop
                    return false;

                return true;
            }

        }
        protected override void Click(Vector3Int gridPosition)
        {
            if (!CanPlaceObjectAt(gridPosition, database.data[selectedObjectIndex].size))
            {
                soundFeedback.PlaySound(SoundType.wrongPlacement);
                return;
            }

            soundFeedback.PlaySound(SoundType.Place);
            int index = objectPlacer.PlaceObject(database.data[selectedObjectIndex].prefab,
                grid.CellToWorld(gridPosition), out var spawnedObject);

            if(placeableType == EPlaceableType.Building)
            {
                gridData.AddObject(gridPosition,
                    database.data[selectedObjectIndex].size,
                    database.data[selectedObjectIndex].id,
                    index);
            }
            else
            {
                var placedObject = objectPlacer.GetObjectAt(gridData.GetGameObjectIndex(gridPosition));
                var soil = placedObject.GetComponent<Soil>();
                soil.AddCrop(spawnedObject.GetComponent<Crop>());

                cropData.AddObject(gridPosition,
                    database.data[selectedObjectIndex].size,
                    database.data[selectedObjectIndex].id,
                    index);
            }

            if (spawnedObject.TryGetComponent<IPlaceable>(out var placeable))
                placeable.OnPlace();

            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
        }

        protected override void Update(Vector3Int gridPosition)
        {
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), CanPlaceObjectAt(gridPosition, database.data[selectedObjectIndex].size));
        }
    }
}