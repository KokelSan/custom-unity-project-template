using System;

public static class UITransitionServiceHandlerData
{
    public static void ShowScreenTransition(int fromScene, int toScene, Action onAnimationCompleted) => OnShowScreenTransition?.Invoke(fromScene, toScene, onAnimationCompleted);
    public static event Action<int, int, Action> OnShowScreenTransition;
    
    public static void HideScreenTransition(Action onAnimationCompleted) => OnHideScreenTransition?.Invoke(onAnimationCompleted);
    public static event Action<Action> OnHideScreenTransition;
}