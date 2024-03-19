using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public struct SceneLoadingParameters
{
    public int SceneIndex;
    public LoadSceneMode LoadSceneMode;
    public ScreenTransitionType ScreenTransitionType; // used if ShowLoadingScreen is false

    public SceneLoadingParameters(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single, ScreenTransitionType screenTransitionType = default)
    {
        SceneIndex = sceneIndex;
        LoadSceneMode = loadSceneMode;
        ScreenTransitionType = screenTransitionType;
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
    public TimeSnapshot StartTime;
    public TimeSnapshot EndTime;
    
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