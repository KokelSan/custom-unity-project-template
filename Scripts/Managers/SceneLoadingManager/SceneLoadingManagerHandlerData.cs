using System;
using UnityEngine.SceneManagement;

public static class SceneLoadingManagerHandlerData
{
    public static void LoadScene(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single, TransitionType transitionType = TransitionType.None, bool isBoot = false) => OnLoadScene?.Invoke(sceneIndex, loadSceneMode, transitionType, isBoot);
    public static event Action<int, LoadSceneMode, TransitionType, bool> OnLoadScene;
    
    public static void UnLoadScene(int sceneIndex) => OnUnLoadScene?.Invoke(sceneIndex);
    public static event Action<int> OnUnLoadScene;
    
    public static void NotifySceneLoaded(int sceneIndex) => OnSceneLoaded?.Invoke(sceneIndex);
    public static event Action<int> OnSceneLoaded;
    
    public static void NotifySceneUnLoaded(int sceneIndex) => OnSceneUnLoaded?.Invoke(sceneIndex);
    public static event Action<int> OnSceneUnLoaded;
    
}