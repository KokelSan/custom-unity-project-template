using UnityEngine;

public class EventDebuggerManager : Manager
{
    protected override void EventHandlerRegister()
    {
        SceneLoadingManagerHandlerData.OnSceneLoaded += OnSceneLoaded;
        SceneLoadingManagerHandlerData.OnSceneUnLoaded += OnSceneUnLoaded;
    }

    protected override void EventHandlerUnRegister()
    {
        SceneLoadingManagerHandlerData.OnSceneLoaded -= OnSceneLoaded;
        SceneLoadingManagerHandlerData.OnSceneUnLoaded -= OnSceneUnLoaded;
    }
    
    private void OnSceneLoaded(int sceneIndex)
    {
        Debug.Log($"SceneLoadingManager: Scene {sceneIndex} successfully loaded");
    }
    
    private void OnSceneUnLoaded(int sceneIndex)
    {
        Debug.Log($"SceneLoadingManager: Scene {sceneIndex} successfully unloaded");
    }
}