using System;
using System.Collections.Generic;
using UnityEngine;

public class UISceneTransitionService : Service
{
    public UISceneTransitionsTableSO TransitionsTableSO;
    public List<UIScreenTransition> ScreenTransitions;

    private UIScreenTransition _pendingScreenTransition;
    
    protected override void EventHandlerRegister()
    {
        UISceneTransitionServiceHandlerData.OnShowScreenTransition += ShowScreenTransition;
        UISceneTransitionServiceHandlerData.OnHideScreenTransition += HideScreenTransition;
    }

    protected override void EventHandlerUnRegister()
    {
        UISceneTransitionServiceHandlerData.OnShowScreenTransition -= ShowScreenTransition;
        UISceneTransitionServiceHandlerData.OnHideScreenTransition -= HideScreenTransition;
    }

    private void ShowScreenTransition(int fromScene, int toScene, Action onAnimationCompleted)
    {
        ScreenTransitionType transitionType = UISceneTransitionsTableUtils.GetTransition(TransitionsTableSO, fromScene, toScene);
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