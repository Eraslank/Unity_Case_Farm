using UnityEngine;

namespace GameCore.GameSystem.Placement
{
    public class PreviewSystem : MonoBehaviour
    {
        [SerializeField] float previewYOffset = 0.06f;
        [SerializeField] GameObject cellIndicator;
        [SerializeField] Material previewMaterialPrefab;

        private GameObject previewObject;

        private Material previewMat;

        private Renderer cellIndicatorRenderer;

        private void Start()
        {
            previewMat = new Material(previewMaterialPrefab);
            cellIndicator.SetActive(false);
            cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
        }

        public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
        {
            previewObject = Instantiate(prefab);
            PreparePreview(previewObject);
            PrepareCursor(size);
            cellIndicator.SetActive(true);
        }
        public void StartShowingRemovePreview()
        {
            cellIndicator.SetActive(true);
            PrepareCursor(Vector2Int.one);
            ApplyFeedbackToCursor(false);
        }
        private void PrepareCursor(Vector2Int size)
        {
            if (size.x > 0 || size.y > 0)
            {
                cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
                cellIndicatorRenderer.material.mainTextureScale = size;
            }
        }

        private void PreparePreview(GameObject previewObject)
        {
            Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = previewMat;
                }
                renderer.materials = materials;
            }
        }

        public void StopShowingPreview()
        {
            cellIndicator.SetActive(false);
            if (previewObject != null)
                Destroy(previewObject);
        }

        public void UpdatePosition(Vector3 position, bool validity)
        {
            if (previewObject != null)
            {
                MovePreview(position);
                ApplyFeedbackToPreview(validity);
            }

            MoveCursor(position);
            ApplyFeedbackToCursor(validity);
        }

        private void ApplyFeedbackToPreview(bool validity) => ApplyFeedback(validity, previewMat);

        private void ApplyFeedbackToCursor(bool validity) => ApplyFeedback(validity, cellIndicatorRenderer.material);

        private void ApplyFeedback(bool valid, Material mat) => mat.color = valid ? Color.white.WithA(.5f) : Color.red.WithA(.5f);

        private void MoveCursor(Vector3 position)
        {
            cellIndicator.transform.position = position;
        }

        private void MovePreview(Vector3 position)
        {
            previewObject.transform.position = new Vector3(
                position.x,
                position.y + previewYOffset,
                position.z);
        }
    }
}