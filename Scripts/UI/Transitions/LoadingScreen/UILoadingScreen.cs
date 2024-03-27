using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UITransition
{
    [Header("Loading Screen Elements")] 
    public Slider ProgressSlider;
    public TMP_Text ProgressText;
    
    public override void PlayShowAnimation(Action onAnimationCompleted = null)
    {
        SceneLoadingService.OnLoadingStarted += OnLoadingStarted;
        SetProgress(0);
        base.PlayShowAnimation(onAnimationCompleted);
    }

    public override void PlayHideAnimation(Action onAnimationCompleted = null)
    {
        SceneLoadingService.OnLoadingStarted -= OnLoadingStarted;
        SetProgress(1);
        base.PlayHideAnimation(onAnimationCompleted);
    }

    private void OnLoadingStarted(AsyncOperation loadingOperation)
    {
        StartCoroutine(UpdateProgress(loadingOperation));
    }

    private IEnumerator UpdateProgress(AsyncOperation loadingOperation)
    {
        while (!loadingOperation.isDone)
        {
            SetProgress(loadingOperation.progress);
            yield return null;
        }
    }

    protected void SetProgress(float progress)
    {
        float adjustedProgress = Mathf.Clamp01(progress / 0.9f - 0.01f); // -0.01f so we stay at 99% until loading is fully done
        ProgressSlider.value = adjustedProgress;
        int progressPercent = (int)(adjustedProgress * 100);
        ProgressText.text = $"{progressPercent}%";
    }
}