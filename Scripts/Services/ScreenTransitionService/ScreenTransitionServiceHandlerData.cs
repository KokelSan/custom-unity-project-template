using System;

public static class ScreenTransitionServiceHandlerData
{
    public static void ShowScreenTransition(ScreenTransitionType screenTransitionType, Action onTransitionCompleted) => OnShowScreenTransition?.Invoke(screenTransitionType, onTransitionCompleted);
    public static event Action<ScreenTransitionType, Action> OnShowScreenTransition;
    
    public static void HideScreenTransition(Action onTransitionCompleted) => OnHideScreenTransition?.Invoke(onTransitionCompleted);
    public static event Action<Action> OnHideScreenTransition;
}