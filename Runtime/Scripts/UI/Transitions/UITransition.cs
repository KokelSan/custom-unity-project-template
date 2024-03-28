using System;
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
        
        public override void PlayShowAnimation(Action onAnimationCompleted = null)
        {
                if (SceneLoadingService.IsLoading)
                {
                        SceneLoadingService.OnSceneLoaded += OnSceneLoaded;
                }
                
                base.PlayShowAnimation(onAnimationCompleted);
        }

        public override void PlayHideAnimation(Action onAnimationCompleted = null)
        {
                if (SceneLoadingService.IsLoading)
                {
                        SceneLoadingService.OnSceneLoaded -= OnSceneLoaded;
                }
                
                base.PlayHideAnimation(onAnimationCompleted);
        }
        
        private void OnSceneLoaded(int sceneIndex, float loadingDuration)
        {
                if(!IsVisible)
                {
                        Debug.LogWarning($"{name} is subscribed to OnSceneLoaded but not visible");
                        return;
                }
        
                CompleteLoading();
        }

        protected virtual void CompleteLoading()
        {
                SceneLoadingService.CompleteLoading();
        }
}