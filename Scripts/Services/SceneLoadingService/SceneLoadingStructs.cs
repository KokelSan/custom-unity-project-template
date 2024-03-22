using UnityEngine;
using UnityEngine.SceneManagement;

public struct SceneLoadingParameters
{
    public int SceneToLoadIndex  { get; private set; }
    public LoadSceneMode LoadSceneMode  { get; private set; }
    
    public SceneLoadingParameters(int sceneToLoadIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneToLoadIndex = sceneToLoadIndex;
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