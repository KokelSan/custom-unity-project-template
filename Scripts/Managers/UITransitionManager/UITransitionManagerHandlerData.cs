using System;

public static class UITransitionManagerHandlerData
{
    public static void ShowSceneTransition(int fromScene, int toScene, Action onAnimationCompleted = null) => OnShowSceneTransition?.Invoke(fromScene, toScene, onAnimationCompleted);
    public static event Action<int, int, Action> OnShowSceneTransition;
    
    public static void ShowTransition(TransitionType transitionType, Action onAnimationCompleted = null) => OnShowTransition?.Invoke(transitionType, onAnimationCompleted);
    public static event Action<TransitionType, Action> OnShowTransition;
    
    public static void PlayFullTransition(TransitionType transitionType, Action onAnimationCompleted = null) => OnPlayFullTransition?.Invoke(transitionType, onAnimationCompleted);
    public static event Action<TransitionType, Action> OnPlayFullTransition;
    
    public static void HideTransition(Action onAnimationCompleted = null) => OnHideTransition?.Invoke(onAnimationCompleted);
    public static event Action<Action> OnHideTransition;
}