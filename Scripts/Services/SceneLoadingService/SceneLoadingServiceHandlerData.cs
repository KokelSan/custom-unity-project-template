using System;

public static class SceneLoadingServiceHandlerData
{
    public static void LoadScene(SceneLoadingParameters parameters) => OnLoadScene?.Invoke(parameters);
    public static event Action<SceneLoadingParameters> OnLoadScene;
    
    public static void UnLoadScene(int sceneIndex) => OnUnLoadScene?.Invoke(sceneIndex);
    public static event Action<int> OnUnLoadScene;
    
    
    public static void SceneLoaded(int sceneIndex, LoadingReport report = default) => OnSceneLoaded?.Invoke(sceneIndex, report);
    public static event Action<int, LoadingReport> OnSceneLoaded;
    
    public static void SceneReadyToPlay(int sceneIndex) => OnSceneReadyToPlay?.Invoke(sceneIndex);
    public static event Action<int> OnSceneReadyToPlay;
    
    
    public static void SceneUnLoaded(int sceneIndex) => OnSceneUnLoaded?.Invoke(sceneIndex);
    public static event Action<int> OnSceneUnLoaded;
    }