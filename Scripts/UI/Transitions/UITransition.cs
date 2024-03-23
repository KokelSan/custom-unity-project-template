using UnityEngine;

public enum TransitionType
{
        Boot,
        LoadingScreen,
        LoadingScreenWaitingForInput,
        Fade,
}

public class UITransition : UIAnimatedElement
{
        [Header("Screen Transition Elements")]
        public TransitionType TransitionType;
}