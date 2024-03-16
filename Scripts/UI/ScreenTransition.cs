using UnityEngine;

public enum TransitionType
{
    None = 0,
    Fade,
    LoadingScreen,
}

[RequireComponent(typeof(Animator))]
public class ScreenTransition : BaseUIElement
{
    public Animator Animator;
    public TransitionType TransitionType;
    
    [HideInInspector] public bool IsAnimating = false;
    private bool _isShown = false;

    protected override void Initialize()
    {
        base.Initialize();
        
        if(Animator == null && !TryGetComponent(out Animator))
        {
            Debug.LogWarning($"Transition '{TransitionType}' (id = {GetInstanceID()}) has no Animator component'");
        }
    }
    
    public void ShowTransition(bool isBoot)
    {
        string triggerName = isBoot ? "Boot" : "Show";
        TriggerAnimator(true, triggerName);
    }
    
    public void HideTransition()
    {
        TriggerAnimator(false, "Hide");
    }

    private void TriggerAnimator(bool show, string triggerName)
    {
        if(Animator == null || IsAnimating || _isShown == show) return;
        _isShown = CanvasGroup.interactable = CanvasGroup.blocksRaycasts = show;
        Animator.SetTrigger(triggerName);
    }
    
    /// <summary>
    /// Called by the animator at the beginning of the AnimationClip
    /// </summary>
    public virtual void OnTransitionStarted()
    {
        ScreenTransitionManagerHandlerData.TransitionStarted();
        IsAnimating = true;
    }
    
    /// <summary>
    /// Called by the animator at the end of the AnimationClip
    /// </summary>
    public virtual void OnTransitionCompleted()
    {
        ScreenTransitionManagerHandlerData.TransitionCompleted();
        IsAnimating = false;
    }
}