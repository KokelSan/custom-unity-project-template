using System;
using UnityEngine.SceneManagement;

public static class SceneLoadingManagerHandlerData
{
    public static void LoadScene(int sceneIndex, bool loadAsync = false) => OnLoadScene?.Invoke(sceneIndex, loadAsync, LoadSceneMode.Single);
    public static void LoadSceneAdditive(int sceneIndex, bool loadAsync = false) => OnLoadScene?.Invoke(sceneIndex, loadAsync, LoadSceneMode.Additive);
    public static event Action<int, bool, LoadSceneMode> OnLoadScene;
    
    public static void UnLoadScene(int sceneIndex) => OnUnLoadScene?.Invoke(sceneIndex);
    public static event Action<int> OnUnLoadScene;
    
    public static void NotifySceneLoaded(int sceneIndex) => OnSceneLoaded?.Invoke(sceneIndex);
    public static event Action<int> OnSceneLoaded;
    
    public static void NotifySceneUnLoaded(int sceneIndex) => OnSceneUnLoaded?.Invoke(sceneIndex);
    public static event Action<int> OnSceneUnLoaded;
    
}