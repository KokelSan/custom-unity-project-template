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

    public bool WaitForInputOnSceneLoaded;
    
    private bool _isWaitingForInput;

    private const string ShowWaitingScreenAnimatorTrigger = "ShowWaitingScreen";

    public override void Show(bool showInstant = false, Action onAnimationCompleted = null)
    {
        SceneLoadingService.OnLoadingStarted += OnLoadingStarted;
        SetProgress(0);
        base.Show(showInstant, onAnimationCompleted);
    }

    public override void Hide(bool hideInstant = false, Action onAnimationCompleted = null)
    {
        SceneLoadingService.OnLoadingStarted -= OnLoadingStarted;
        SetProgress(1);
        base.Hide(hideInstant, onAnimationCompleted);
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
    
    protected override void CompleteLoading()
    {
        SetProgress(1);
        
        if (WaitForInputOnSceneLoaded)
        {
            _isWaitingForInput = true;
            Animator.SetTrigger(ShowWaitingScreenAnimatorTrigger);
            return;
        }
        
        SceneLoadingService.CompleteLoading();
    }
    
    public override void OnAnimationCompleted()
    {
        base.OnAnimationCompleted();
        if (!WaitForInputOnSceneLoaded || !_isWaitingForInput) return;
        
        InputManagerHandlerData.OnClick += OnClick;
    }

    private void OnClick(Vector2 _)
    {
        InputManagerHandlerData.OnClick -= OnClick;
        
        SceneLoadingService.CompleteLoading();
        _isWaitingForInput = false;
    }
}