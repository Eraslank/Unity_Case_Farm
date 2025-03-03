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

        public PlacementState(int iD,
                              Grid grid,
                              PreviewSystem previewSystem,
                              ObjectsDatabaseSO database,
                              GridData gridData,
                              ObjectPlacer objectPlacer,
                              SoundFeedback soundFeedback) : base(grid, previewSystem, gridData, objectPlacer, soundFeedback)
        {
            ID = iD;
            this.database = database;

            selectedObjectIndex = database.data.FindIndex(data => data.id == ID);
            if (selectedObjectIndex > -1)
            {
                previewSystem.StartShowingPlacementPreview(
                    database.data[selectedObjectIndex].prefab,
                    database.data[selectedObjectIndex].size);
            }
            else
                throw new System.Exception($"No object with ID {iD}");

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
                grid.CellToWorld(gridPosition));

            gridData.AddObject(gridPosition,
                database.data[selectedObjectIndex].size,
                database.data[selectedObjectIndex].id,
                index);

            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
        }

        protected override void Update(Vector3Int gridPosition)
        {
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), CanPlaceObjectAt(gridPosition, database.data[selectedObjectIndex].size));
        }
    }
}