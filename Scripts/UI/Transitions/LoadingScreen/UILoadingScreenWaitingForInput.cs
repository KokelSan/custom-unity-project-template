using System;
using UnityEngine;

public class UILoadingScreenWaitingForInput : UILoadingScreen
{
    [Header("Input waiting Elements")] 
    private bool _isWaitingForInput;
    private Action _onScreenClickedWhenWaiting;

    private const string ShowWaitingScreenAnimatorTrigger = "ShowWaitingScreen";

    public override void PlayShowAnimation(Action _)
    {
        UILoadingScreenHandlerData.OnShouldWaitForInput += ShouldWaitForInput;
        UILoadingScreenHandlerData.OnWaitForInput += WaitForInput;
        base.PlayShowAnimation(_);
    }

    public override void PlayHideAnimation(Action onAnimationCompleted)
    {
        UILoadingScreenHandlerData.OnShouldWaitForInput -= ShouldWaitForInput;
        UILoadingScreenHandlerData.OnWaitForInput -= WaitForInput;
        base.PlayHideAnimation(onAnimationCompleted);
    }

    private bool ShouldWaitForInput() => true;
    
    private void WaitForInput(Action onScreenClicked)
    {
        OnProgressUpdated(1);
        _onScreenClickedWhenWaiting = onScreenClicked;
        _isWaitingForInput = true;
        Animator.SetTrigger(ShowWaitingScreenAnimatorTrigger);
    }

    public override void OnAnimationCompleted()
    {
        base.OnAnimationCompleted();
        if (!_isWaitingForInput) return;
        
        InputManagerHandlerData.OnClick += OnClick;
    }

    private void OnClick(Vector2 _)
    {
        if (!_isWaitingForInput) return;
        
        InputManagerHandlerData.OnClick -= OnClick;
        _onScreenClickedWhenWaiting?.Invoke();
        _isWaitingForInput = false;
    }
}