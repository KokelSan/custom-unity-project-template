using System;

public static class ScreenTransitionManagerHandlerData
{
    public static void ShowTransition(TransitionType transitionType, bool isBoot = false, Action<float> onTransitionStarted = null) => OnShowTransition?.Invoke(transitionType, isBoot, onTransitionStarted);
    public static event Action<TransitionType, bool, Action<float>> OnShowTransition;
    
    public static void HideTransition(TransitionType transitionType, Action<float> onTransitionStarted = null) => OnHideTransition?.Invoke(transitionType, onTransitionStarted);
    public static event Action<TransitionType, Action<float>> OnHideTransition;
    
}