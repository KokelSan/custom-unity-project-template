using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitionManager : Manager
{
    public List<ScreenTransition> ScreenTransitions;
    private List<ScreenTransition> _transitions = new List<ScreenTransition>();
    
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

    protected override void Initialize()
    {
        base.Initialize();
        foreach (ScreenTransition transition in ScreenTransitions)
        {
            _transitions.Add(Instantiate(transition, transform));
        }
    }

    #endregion

    private void ShowTransition(TransitionType transitionType, bool isBoot)
    {
        if (TryGetScreenTransition(transitionType, out ScreenTransition screenTransition))
        {
            screenTransition.ShowTransition(isBoot);
        }
    }

    private void HideTransition(TransitionType transitionType)
    {
        if (TryGetScreenTransition(transitionType, out ScreenTransition screenTransition))
        {
            screenTransition.HideTransition();
        }
    }

    private bool TryGetScreenTransition(TransitionType transitionType, out ScreenTransition screenTransition)
    {
        screenTransition = _transitions.Find(transition => transition.TransitionType == transitionType);
        if (screenTransition == null)
        {
            Debug.LogWarning("Trying to access an unknown ScreenTransition");
        }
        return screenTransition != null;
    }
}