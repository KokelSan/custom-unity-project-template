using UnityEngine;

public class LoadingScreenUI : ScreenTransition
{
    public override void OnTransitionCompleted()
    {
        base.OnTransitionCompleted();
        if(!_isShown) return;
        InputManagerHandlerData.OnClick += OnClick;
        // Debug.Log("Loading screen ready to be clicked");
    }

    private void OnClick(Vector2 _)
    {
        // Debug.Log("Loading screen clicked");
        InputManagerHandlerData.OnClick -= OnClick;
        ScreenTransitionManagerHandlerData.LoadingScreenClicked();
    }
}