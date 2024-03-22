using UnityEngine;

public enum ScreenTransitionType
{
        Boot,
        LoadingScreen,
        LoadingScreenWaitingForInput,
        Fade,
}

public class UIScreenTransition : UIAnimatedElement
{
        [Header("Screen Transition Elements")]
        public ScreenTransitionType TransitionType;
}