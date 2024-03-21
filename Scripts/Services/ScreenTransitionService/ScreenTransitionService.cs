using System;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenTransitionType
{
    Boot,
    LoadingScreen,
    Fade,
}

public class ScreenTransitionService : Service
{
    public List<UIScreenTransition> ScreenTransitions;

    private UIScreenTransition _pendingScreenTransition;
    
    protected override void EventHandlerRegister()
    {
        ScreenTransitionServiceHandlerData.OnShowScreenTransition += ShowScreenTransition;
        ScreenTransitionServiceHandlerData.OnHideScreenTransition += HideScreenTransition;
    }

    protected override void EventHandlerUnRegister()
    {
        ScreenTransitionServiceHandlerData.OnShowScreenTransition -= ShowScreenTransition;
        ScreenTransitionServiceHandlerData.OnHideScreenTransition -= HideScreenTransition;
    }

    private void ShowScreenTransition(ScreenTransitionType transitionType, Action onAnimationCompleted)
    {
        UIScreenTransition screenTransition = ScreenTransitions.Find(screen => screen.TransitionType == transitionType);
        if (screenTransition != null)
        {
            screenTransition.PlayShowAnimation(onAnimationCompleted);
            _pendingScreenTransition = screenTransition;
            return;
        }
        Debug.LogWarning("Trying to show an unregistered transition");
    }
    
    private void HideScreenTransition(Action onAnimationCompleted)
    {
        if (_pendingScreenTransition != null)
        {
            _pendingScreenTransition.PlayHideAnimation(onAnimationCompleted);
            _pendingScreenTransition = null;
            return;
        }
        Debug.LogWarning("Trying to hide a not-shown transition");
    }
}