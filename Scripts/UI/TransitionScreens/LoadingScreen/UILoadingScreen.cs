using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIScreenTransition
{
    [Header("Loading Screen Elements")] 
    public Slider ProgressSlider;
    public TMP_Text ProgressText;

    private bool _isWaitingForInput;
    private Action _onScreenClickedWhenWaiting;

    private const string WaitingForInputAnimatorTrigger = "WaitingForInput";

    protected override void EventHandlerRegister()
    {
        LoadingScreenHandlerData.OnLoadingProgressUpdate += OnProgressUpdated;
        LoadingScreenHandlerData.OnWaitForInput += WaitForInput;
    }

    protected override void EventHandlerUnRegister()
    {
        LoadingScreenHandlerData.OnLoadingProgressUpdate -= OnProgressUpdated;
        LoadingScreenHandlerData.OnWaitForInput -= WaitForInput;
    }

    public override void Show(Action _)
    {
        base.Show(_);
        OnProgressUpdated(0);
    }

    private void OnProgressUpdated(float progress)
    {
        ProgressSlider.value = progress;
        ProgressText.text = $"{(int)(progress * 100) + 1}%";
    }

    private void WaitForInput(Action onScreenClicked)
    {
        _onScreenClickedWhenWaiting = onScreenClicked;
        _isWaitingForInput = true;
        Animator.SetTrigger(WaitingForInputAnimatorTrigger);
    }

    public override void OnAnimationCompleted()
    {
        base.OnAnimationCompleted();
        if (!_isWaitingForInput) return;
        
        InputServiceHandlerData.OnClick += OnClick;
    }

    private void OnClick(Vector2 _)
    {
        if (!_isWaitingForInput) return;
        
        InputServiceHandlerData.OnClick -= OnClick;
        _onScreenClickedWhenWaiting?.Invoke();
        _isWaitingForInput = false;
    }
}