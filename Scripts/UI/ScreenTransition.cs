using System;
using System.Collections;
using UnityEngine;

public enum TransitionType
{
    None = 0,
    Fade,
}

[RequireComponent(typeof(Animator))]
public class ScreenTransition : MonoBehaviour
{
    public Animator Animator;
    public TransitionType TransitionType;
    
    [HideInInspector] public bool IsAnimating = false;
    private bool _isShown = false;
    
    private void Start()
    {
        if(Animator == null && !TryGetComponent(out Animator))
        {
            Debug.LogWarning($"Transition '{TransitionType}' (id = {GetInstanceID()}) has no Animator component'");
        }
    }

    /// <summary>
    /// Show the transition to hide the screen
    /// </summary>
    public void Show(bool isBoot)
    {
        string triggerName = isBoot ? "Boot" : "Show";
        TriggerAnimator(true, triggerName);
    }

    /// <summary>
    /// Hide the transition to show the screen
    /// </summary>
    public void Hide()
    {
        TriggerAnimator(false, "Hide");
    }

    private void TriggerAnimator(bool show, string triggerName)
    {
        if(Animator == null || IsAnimating || _isShown == show)
        {
            Debug.LogError($"Animator null = {Animator == null}, IsAnimating = {IsAnimating}, _isShown = {_isShown}, shown = {show}");
            return;
        }
        Animator.SetTrigger(triggerName);
        _isShown = show;
    }
    
    
    public void OnTransitionStarted()
    {
        ScreenTransitionManagerHandlerData.TransitionStarted();
    }
    
    public void OnTransitionCompleted()
    {
        ScreenTransitionManagerHandlerData.TransitionCompleted();
    }
}