using System;

public static class ScreenTransitionManagerHandlerData
{
    public static void ShowTransition(TransitionType transitionType, bool isBoot = false) => OnShowTransition?.Invoke(transitionType, isBoot);
    public static event Action<TransitionType, bool> OnShowTransition;
    
    public static void HideTransition(TransitionType transitionType) => OnHideTransition?.Invoke(transitionType);
    public static event Action<TransitionType> OnHideTransition;
    
}