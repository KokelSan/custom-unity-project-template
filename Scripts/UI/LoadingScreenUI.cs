using UnityEngine;

public class LoadingScreenUI : ScreenTransition
{
    public override void OnTransitionCompleted()
    {
        base.OnTransitionCompleted();
        InputManagerHandlerData.OnClick += OnClick;
        // Debug.Log("Transition Completed");
    }

    private void OnClick(Vector2 _)
    {
        // Debug.Log("LoadingScreenClicked");
        InputManagerHandlerData.OnClick -= OnClick;
        ScreenTransitionManagerHandlerData.LoadingScreenClicked();
    }
}