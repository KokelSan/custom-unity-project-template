using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct TimeSnapshot
{
    public float Value { get; private set; }
    
    public void Set()
    {
        Value = Time.realtimeSinceStartup;
    }

    public TimeSnapshot(float value)
    {
        Value = 0;
    }
}

[Serializable]
public class SceneLoadingData
{
    public int SceneToLoadIndex;
    public LoadSceneMode LoadSceneMode;
    
    private TimeSnapshot _startTime;
    private TimeSnapshot _endTime;
    public float Duration => Mathf.Max(0, _endTime.Value - _startTime.Value);
    
    public SceneLoadingData(int sceneToLoadIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneToLoadIndex = sceneToLoadIndex;
        LoadSceneMode = loadSceneMode;
        _startTime = new TimeSnapshot(-1);
        _endTime = new TimeSnapshot(-1);
    }

    public void StartDurationRecord()
    {
        _startTime.Set();
    }

    public void StopDurationRecord()
    {
        _endTime.Set();
    }
}