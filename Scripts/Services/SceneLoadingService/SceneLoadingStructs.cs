using UnityEngine;
using UnityEngine.SceneManagement;

public struct SceneLoadingParameters
{
    public int SceneIndex  { get; private set; }
    public ScreenTransitionType ScreenTransitionType  { get; private set; }
    public LoadSceneMode LoadSceneMode  { get; private set; }

    public bool HasLoadingScreen => ScreenTransitionType is 
        ScreenTransitionType.LoadingScreen or 
        ScreenTransitionType.LoadingScreenWaitingForInput;

    public bool ShouldWaitForInputAfterLoading => ScreenTransitionType is
        ScreenTransitionType.LoadingScreenWaitingForInput;

    public SceneLoadingParameters(int sceneIndex, ScreenTransitionType screenTransitionType = ScreenTransitionType.LoadingScreen, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneIndex = sceneIndex;
        ScreenTransitionType = screenTransitionType;
        LoadSceneMode = loadSceneMode;
    }
}

public struct TimeSnapshot
{
    public float Value { get; private set; }
    
    public void Set()
    {
        Value = Time.realtimeSinceStartup;
    }
}

public struct LoadingReport
{
    private TimeSnapshot _startTime;
    private TimeSnapshot _endTime;
    
    public float Duration => Mathf.Max(0, _endTime.Value - _startTime.Value);
    
    public void Start()
    {
        _startTime.Set();
    }

    public void Stop()
    {
        _endTime.Set();
    }
}