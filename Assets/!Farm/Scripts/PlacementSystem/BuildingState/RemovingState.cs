using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.GameSystem.Placement
{
    public class RemovingState : BuildingState
    {
        public RemovingState(Grid grid,
                             PreviewSystem previewSystem,
                             GridData gridData,
                             GridData cropData,
                             ObjectPlacer objectPlacer)
            : base(grid, previewSystem, gridData, cropData, objectPlacer)
        {
            previewSystem.StartShowingRemovePreview();
        }
        protected override void Click(Vector3Int gridPosition)
        {
            if (CanPlaceObjectAt(gridPosition, Vector2Int.one))
            {
                return;
            }

            var gameObjectIndex = gridData.GetGameObjectIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;

            gridData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);

            var cropObjectIndex = cropData.GetGameObjectIndex(gridPosition);
            if (cropObjectIndex != -1)
            {
                cropData.RemoveObjectAt(gridPosition);
                objectPlacer.RemoveObjectAt(cropObjectIndex);
            }

            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdatePosition(cellPosition, !CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }
        protected override void Update(Vector3Int gridPosition)
        {
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), !CanPlaceObjectAt(gridPosition, Vector2Int.one));
        }
    }
}