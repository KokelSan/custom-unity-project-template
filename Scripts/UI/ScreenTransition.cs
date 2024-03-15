using System;
using System.Collections;
using Unity.VisualScripting;
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
    public float TransitionDuration = 1;
    
    [HideInInspector] public bool IsAnimating = false;
    private bool _isShown = false;
    
    private void Start()
    {
        if(Animator == null && !TryGetComponent(out Animator))
        {
            Debug.LogWarning($"Transition '{TransitionType}' (id = {GetInstanceID()}) has no Animator component'");
            return;
        }
    }

    /// <summary>
    /// Show the transition to hide the screen
    /// </summary>
    public void Show(bool isBoot, Action<float> onTransitionStarted)
    {
        string triggerName = isBoot ? "Boot" : "Show";
        StartCoroutine(TriggerAnimator(true, triggerName, onTransitionStarted));
    }

    /// <summary>
    /// Hide the transition to show the screen
    /// </summary>
    public void Hide(Action<float> onTransitionStarted)
    {
        StartCoroutine(TriggerAnimator(false, "Hide", onTransitionStarted));
    }

    IEnumerator TriggerAnimator(bool isShown, string triggerName, Action<float> onTransitionStarted)
    {
        if(Animator == null || IsAnimating || isShown == _isShown) yield break;
        
        AnimatorStateInfo oldState = Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo newState = oldState;
        Animator.SetTrigger(triggerName);
        while (newState.Equals(oldState))
        {
            newState = Animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        onTransitionStarted?.Invoke(newState.length);
        _isShown = isShown;
    }
}