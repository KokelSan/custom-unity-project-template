using System;

public static class SceneLoadingServiceHandlerData
{
    public static void LoadScene(SceneLoadingData data) => OnLoadScene?.Invoke(data);
    public static event Action<SceneLoadingData> OnLoadScene;
    
    public static void UnLoadScene(int sceneIndex) => OnUnLoadScene?.Invoke(sceneIndex);
    public static event Action<int> OnUnLoadScene;
    
    
    public static void SceneLoaded(int sceneIndex, SceneLoadingData data) => OnSceneLoaded?.Invoke(sceneIndex, data);
    public static event Action<int, SceneLoadingData> OnSceneLoaded;
    
    public static void SceneReadyToPlay(int sceneIndex) => OnSceneReadyToPlay?.Invoke(sceneIndex);
    public static event Action<int> OnSceneReadyToPlay;
    
    
    public static void SceneUnLoaded(int sceneIndex) => OnSceneUnLoaded?.Invoke(sceneIndex);
    public static event Action<int> OnSceneUnLoaded;

    
    public static int GetActiveScene() => OnGetActiveScene?.Invoke() ?? 0;
    public static event Func<int> OnGetActiveScene;

}