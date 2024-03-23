using UnityEngine;

public enum ScreenTransitionType
{
        Boot,
        LoadingScreen,
        LoadingScreenWaitingForInput,
        Fade,
}

public class UITransition : UIAnimatedElement
{
        [Header("Screen Transition Elements")]
        public ScreenTransitionType TransitionType;
}