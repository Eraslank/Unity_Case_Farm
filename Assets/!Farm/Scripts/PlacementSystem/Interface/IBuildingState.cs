using UnityEngine;

namespace GameCore.GameSystem.Placement
{
    public interface IBuildingState
    {
        void EndState();
        void OnClick(Vector3Int gridPosition);
        void UpdateState(Vector3Int gridPosition);
    }
}