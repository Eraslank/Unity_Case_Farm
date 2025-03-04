using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour, IPlaceable
{
    [SerializeField] Transform meshParent;
    [SerializeField] GameObject[] stages;

    [SerializeField] Vector3 maxGrowth = Vector3.one;

    Tween growthTween;

    private bool readyToHarvest = false;
    public bool ReadyToHarvest => readyToHarvest;

    private void OnDestroy()
    {
        growthTween?.Kill();
        CropManager.Instance.UnRegisterCrop(this);
    }

    public void OnPlace()
    {
        StartGrowing();
    }

    private void StartGrowing()
    {
        CropManager.Instance.RegisterCrop(this);

        growthTween = meshParent.DOScale(maxGrowth, 10f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            var threshold = 1f / (float)stages.Length;

            SetStage((int)(growthTween.ElapsedPercentage() / threshold));
        }).OnComplete(() => readyToHarvest = true);
    }

    private void SetStage(int stage)
    {
        stage = Mathf.Clamp(stage, 0, stages.Length - 1);

        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].gameObject.SetActive(stage == i);
        }
    }
}
