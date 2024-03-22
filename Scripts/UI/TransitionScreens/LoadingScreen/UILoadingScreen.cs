using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIScreenTransition
{
    [Header("Loading Screen Elements")] 
    public Slider ProgressSlider;
    public TMP_Text ProgressText;
    
    public override void PlayShowAnimation(Action _)
    {
        UILoadingScreenHandlerData.OnLoadingProgressUpdate += OnProgressUpdated;
        OnProgressUpdated(0);
        base.PlayShowAnimation(_);
    }

    public override void PlayHideAnimation(Action onAnimationCompleted)
    {
        UILoadingScreenHandlerData.OnLoadingProgressUpdate -= OnProgressUpdated;
        OnProgressUpdated(1);
        base.PlayHideAnimation(onAnimationCompleted);
    }
    

    protected void OnProgressUpdated(float progress)
    {
        float adjustedProgress = Mathf.Clamp01(progress / 0.9f - 0.01f); // -0.01f so we stay at 99% until loading is fully done
        ProgressSlider.value = adjustedProgress;
        int progressPercent = (int)(adjustedProgress * 100);
        ProgressText.text = $"{progressPercent}%";
    }
}