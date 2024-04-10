using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimatedElement : UIBaseElement
{
    [Header("UI Animated Element")]
    public Animator Animator;
    
    [HideInInspector] public bool IsAnimating = false;

    private Action _onAnimationCompleted;
    
    private const string ShowAnimatorTrigger = "Show";
    private const string HideAnimatorTrigger = "Hide";
    private const string InstantShowAnimatorTrigger = "InstantShow";
    private const string InstantHideAnimatorTrigger = "InstantHide";
    
    protected override void Initialize()
    {
        base.Initialize();
        
        if(Animator == null && !TryGetComponent(out Animator))
        {
            Debug.LogWarning($"UIAnimatedElement called '{name}' (id = {GetInstanceID()}) doesn't have the required Animator component");
        }
    }

    private bool CanPlayAnimation(bool show)
    {
        return Animator != null && !IsAnimating && IsVisible != show;
    }
    
    public virtual void Show (bool showInstant = false, Action onAnimationCompleted = null)
    {
        string trigger = showInstant ? InstantShowAnimatorTrigger : ShowAnimatorTrigger;
        TriggerAnimator(true, trigger, onAnimationCompleted);
    }
    
    public virtual void Hide (bool hideInstant = false, Action onAnimationCompleted = null)
    {
        string trigger = hideInstant ? InstantHideAnimatorTrigger : HideAnimatorTrigger;
        TriggerAnimator(false, trigger, onAnimationCompleted);
    }

    private void TriggerAnimator(bool visible, string triggerName, Action onAnimationCompleted)
    {
        if(!CanPlayAnimation(visible)) return;
        
        IsVisible = visible;
        CanvasGroup.interactable = CanvasGroup.blocksRaycasts = visible;
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
        Action onAnimationCompletedTmp = _onAnimationCompleted;
        _onAnimationCompleted?.Invoke();
        
        // To prevent null the action if the invoke calls a play/hide on the same object
        if (_onAnimationCompleted == onAnimationCompletedTmp)
        {
            _onAnimationCompleted = null;
        }
    }
}