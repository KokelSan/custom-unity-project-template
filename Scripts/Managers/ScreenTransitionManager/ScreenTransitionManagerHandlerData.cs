using System;

public static class ScreenTransitionManagerHandlerData
{
    public static void ShowTransition(TransitionType transitionType, bool isBoot = false) => OnShowTransition?.Invoke(transitionType, isBoot);
    public static event Action<TransitionType, bool> OnShowTransition;
    
    public static void HideTransition(TransitionType transitionType) => OnHideTransition?.Invoke(transitionType);
    public static event Action<TransitionType> OnHideTransition;


    public static void TransitionStarted() => OnTransitionStarted?.Invoke();
    public static event Action OnTransitionStarted;

    public static void TransitionCompleted() => OnTransitionCompleted?.Invoke();
    public static event Action OnTransitionCompleted;


    public static void LoadingScreenClicked() => OnLoadingScreenClicked?.Invoke();
    public static event Action OnLoadingScreenClicked;
}