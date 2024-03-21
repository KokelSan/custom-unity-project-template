using System;

public static class ScreenTransitionServiceHandlerData
{
    public static void ShowScreenTransition(ScreenTransitionType screenTransitionType, Action onAnimationCompleted) => OnShowScreenTransition?.Invoke(screenTransitionType, onAnimationCompleted);
    public static event Action<ScreenTransitionType, Action> OnShowScreenTransition;
    
    public static void HideScreenTransition(Action onAnimationCompleted) => OnHideScreenTransition?.Invoke(onAnimationCompleted);
    public static event Action<Action> OnHideScreenTransition;
}