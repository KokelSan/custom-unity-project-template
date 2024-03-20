using UnityEngine.SceneManagement;

public struct SceneLoadingParameters
{
    public int SceneIndex  { get; private set; }
    public ScreenTransitionType ScreenTransitionType  { get; private set; }
    public LoadSceneMode LoadSceneMode  { get; private set; }

    public bool HasLoadingScreen => ScreenTransitionType == ScreenTransitionType.LoadingScreen;

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
    public bool IsNull { get; private set; }
    
    public void Set()
    {
        Value = UnityEngine.Time.realtimeSinceStartup;
        IsNull = false;
    }

    public void Reset()
    {
        IsNull = true;
    }
}

public struct LoadingReport
{
    public TimeSnapshot StartTime { get; private set; }
    public TimeSnapshot EndTime { get; private set; }
    
    public float Duration => EndTime.Value - StartTime.Value;
    
    public void Start()
    {
        StartTime.Set();
        EndTime.Reset();
    }

    public void Finish()
    {
        EndTime.Set();
    }
}