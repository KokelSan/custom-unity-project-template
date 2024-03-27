using UnityEngine;

public class UILoadingScreenWaitingForInput : UILoadingScreen
{
    [Header("Input waiting Elements")] 
    private bool _isWaitingForInput;

    private const string ShowWaitingScreenAnimatorTrigger = "ShowWaitingScreen";

    protected override void CompleteLoading()
    {
        SetProgress(1);
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
        
        SceneLoadingService.CompleteLoading();
        _isWaitingForInput = false;
    }
}