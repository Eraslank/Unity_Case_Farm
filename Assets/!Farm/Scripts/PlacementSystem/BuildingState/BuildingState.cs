using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCore.GameSystem.Placement
{
    public abstract class BuildingState : IBuildingState
    {
        protected Grid grid;
        protected PreviewSystem previewSystem;
        protected GridData gridData;
        protected GridData cropData;
        protected ObjectPlacer objectPlacer;
        protected SoundFeedback soundFeedback;

        public BuildingState(Grid grid,
                             PreviewSystem previewSystem,
                             GridData gridData,
                             GridData cropData,
                             ObjectPlacer objectPlacer,
                             SoundFeedback soundFeedback)
        {
            this.grid = grid;
            this.previewSystem = previewSystem;
            this.gridData = gridData;
            this.cropData = cropData;
            this.objectPlacer = objectPlacer;
            this.soundFeedback = soundFeedback;
        }

        protected virtual bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int size)
        {
            return gridData.CanPlaceObjectAt(gridPosition, size);
        }
        public virtual void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnClick(Vector3Int gridPosition)
        {
            Click(gridPosition);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            Update(gridPosition);
        }

        protected abstract void Click(Vector3Int gridPosition);
        protected abstract void Update(Vector3Int gridPosition);
    }
}