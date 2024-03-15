using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitionManager : Manager
{
    public List<ScreenTransition> ScreenTransitions;
    
    #region Overrides

    protected override void EventHandlerRegister()
    {
        ScreenTransitionManagerHandlerData.OnShowTransition += ShowTransition;
        ScreenTransitionManagerHandlerData.OnHideTransition += HideTransition;
    }

    protected override void EventHandlerUnRegister()
    {
        ScreenTransitionManagerHandlerData.OnShowTransition -= ShowTransition;
        ScreenTransitionManagerHandlerData.OnHideTransition -= HideTransition;
    }

    #endregion

    private void ShowTransition(TransitionType transitionType, bool isBoot, Action<float> onTransitionStarted)
    {
        if (TryGetScreenTransition(transitionType, out ScreenTransition screenTransition))
        {
            screenTransition.Show(isBoot, onTransitionStarted);
        }
    }

    private void HideTransition(TransitionType transitionType, Action<float> onTransitionStarted)
    {
        if (TryGetScreenTransition(transitionType, out ScreenTransition screenTransition))
        {
            screenTransition.Hide(onTransitionStarted);
        }
    }

    private bool TryGetScreenTransition(TransitionType transitionType, out ScreenTransition screenTransition)
    {
        screenTransition = ScreenTransitions.Find(transition => transition.TransitionType == transitionType);
        if (screenTransition == null)
        {
            Debug.LogWarning("Trying to access an unknown ScreenTransition");
        }
        return screenTransition != null;
    }
}