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

    private void ShowTransition(TransitionType transitionType, bool isBoot)
    {
        if (TryGetScreenTransition(transitionType, out ScreenTransition screenTransition))
        {
            screenTransition.Show(isBoot);
        }
    }

    private void HideTransition(TransitionType transitionType)
    {
        if (TryGetScreenTransition(transitionType, out ScreenTransition screenTransition))
        {
            screenTransition.Hide();
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