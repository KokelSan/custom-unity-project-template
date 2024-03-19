using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIScreenTransition
{
    [Header("Loading Screen Elements")] 
    public Slider ProgressSlider;
    public TMP_Text ProgressText;

    protected override void EventHandlerRegister()
    {
        LoadingScreenHandlerData.OnLoadingProgressUpdate += OnProgressUpdated;
    }

    protected override void EventHandlerUnRegister()
    {
        LoadingScreenHandlerData.OnLoadingProgressUpdate -= OnProgressUpdated;
    }

    public override void OnAnimationCompleted()
    {
        base.OnAnimationCompleted();
        
        if(!IsShown) return;
        InputServiceHandlerData.OnClick += OnClick;
    }

    private void OnProgressUpdated(float progress)
    {
        Debug.LogWarning($"Progress: {progress}");
    }

    private void OnClick(Vector2 _)
    {
        InputServiceHandlerData.OnClick -= OnClick;
    }
}