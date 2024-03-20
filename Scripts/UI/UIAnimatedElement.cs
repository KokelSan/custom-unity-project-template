using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimatedElement : UIBaseElement
{
    [Header("UI Animated Element")]
    public Animator Animator;
    
    [HideInInspector] public bool IsAnimating = false;
    protected bool IsShown;

    private Action _onAnimationCompleted;
    
    private const string ShowAnimatorTrigger = "Show";
    private const string HideAnimatorTrigger = "Hide";
    
    protected override void Initialize()
    {
        base.Initialize();
        
        if(Animator == null && !TryGetComponent(out Animator))
        {
            Debug.LogWarning($"UIAnimatedElement called '{name}' (id = {GetInstanceID()}) doesn't have the required Animator component");
        }
    }

    public virtual void Show (Action onAnimationCompleted)
    {
        TriggerAnimator(true, ShowAnimatorTrigger, onAnimationCompleted);
    }
    
    public virtual void Hide (Action onAnimationCompleted)
    {
        TriggerAnimator(false, HideAnimatorTrigger, onAnimationCompleted);
    }

    private void TriggerAnimator(bool show, string triggerName, Action onAnimationCompleted)
    {
        if(Animator == null || IsAnimating || IsShown == show) return;
        
        IsShown = show;
        CanvasGroup.interactable = CanvasGroup.blocksRaycasts = show;
        _onAnimationCompleted = onAnimationCompleted;
        Animator.SetTrigger(triggerName);
    }
    
    public virtual void OnAnimationStarted()
    {
        IsAnimating = true;
    }
    
    public virtual void OnAnimationCompleted()
    {
        IsAnimating = false;
        _onAnimationCompleted?.Invoke();
        _onAnimationCompleted = null;
    }
}