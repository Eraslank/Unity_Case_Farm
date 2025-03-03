using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameSystem.Placement
{
    public class RemovingState : BuildingState
    {
        private int gameObjectIndex = -1;

        public RemovingState(Grid grid, PreviewSystem previewSystem, GridData gridData, ObjectPlacer objectPlacer, SoundFeedback soundFeedback)
            : base(grid, previewSystem, gridData, objectPlacer, soundFeedback)
        {
            previewSystem.StartShowingRemovePreview();
        }
        protected override void Click(Vector3Int gridPosition)
        {
            if (CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                soundFeedback.PlaySound(SoundType.wrongPlacement);
                return;
            }

            gameObjectIndex = gridData.GetGameObjectIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;

            soundFeedback.PlaySound(SoundType.Remove);
            gridData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdatePosition(cellPosition, !CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }
        protected override void Update(Vector3Int gridPosition)
        {
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), !CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }
    }
}