using System;
using System.Collections.Generic;
using UnityEngine;

public class UITransitionService : Service
{
    public UISceneTransitionsTableSO TransitionsTableSO;
    public List<UITransition> ScreenTransitions;

    private UITransition _pendingTransition;
    
    protected override void EventHandlerRegister()
    {
        UITransitionServiceHandlerData.OnShowScreenTransition += ShowScreenTransition;
        UITransitionServiceHandlerData.OnHideScreenTransition += HideScreenTransition;
    }

    protected override void EventHandlerUnRegister()
    {
        UITransitionServiceHandlerData.OnShowScreenTransition -= ShowScreenTransition;
        UITransitionServiceHandlerData.OnHideScreenTransition -= HideScreenTransition;
    }

    private void ShowScreenTransition(int fromScene, int toScene, Action onAnimationCompleted)
    {
        ScreenTransitionType transitionType = UISceneTransitionsTableUtils.GetTransition(TransitionsTableSO, fromScene, toScene);
        UITransition transition = ScreenTransitions.Find(screen => screen.TransitionType == transitionType);
        if (transition != null)
        {
            transition.PlayShowAnimation(onAnimationCompleted);
            _pendingTransition = transition;
            return;
        }
        Debug.LogWarning("Trying to show an unregistered transition");
    }
    
    private void HideScreenTransition(Action onAnimationCompleted)
    {
        if (_pendingTransition != null)
        {
            _pendingTransition.PlayHideAnimation(onAnimationCompleted);
            _pendingTransition = null;
            return;
        }
        Debug.LogWarning("Trying to hide a not-shown transition");
    }
}