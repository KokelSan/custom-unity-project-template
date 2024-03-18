using System;
using UnityEngine.SceneManagement;

public static class SceneLoadingManagerHandlerData
{
    public static void LoadBootScene(int sceneIndex, TransitionType transitionType) => OnLoadScene?.Invoke(sceneIndex, LoadSceneMode.Single, transitionType, true);
    public static void LoadScene(int sceneIndex, LoadSceneMode loadSceneMode, TransitionType transitionType) => OnLoadScene?.Invoke(sceneIndex, loadSceneMode, transitionType, false);
    public static void LoadScene(int sceneIndex, LoadSceneMode loadSceneMode) => OnLoadScene?.Invoke(sceneIndex, loadSceneMode, TransitionType.None, false);
    public static void LoadScene(int sceneIndex, TransitionType transitionType) => OnLoadScene?.Invoke(sceneIndex, LoadSceneMode.Single, transitionType, false);
    public static void LoadScene(int sceneIndex) => OnLoadScene?.Invoke(sceneIndex, LoadSceneMode.Single, TransitionType.None, false);
    public static event Action<int, LoadSceneMode, TransitionType, bool> OnLoadScene;
    
    
    public static void UnLoadScene(int sceneIndex) => OnUnLoadScene?.Invoke(sceneIndex);
    public static event Action<int> OnUnLoadScene;
    
    
    public static void SceneLoaded(int sceneIndex, LoadingReport report = default) => OnSceneLoaded?.Invoke(sceneIndex, report);
    public static event Action<int, LoadingReport> OnSceneLoaded;
    
    public static void SceneReady(int sceneIndex, LoadingReport report) => OnSceneReady?.Invoke(sceneIndex, report);
    public static event Action<int, LoadingReport> OnSceneReady;
    
    public static void SceneUnLoaded(int sceneIndex) => OnSceneUnLoaded?.Invoke(sceneIndex);
    public static event Action<int> OnSceneUnLoaded;


    public static void UpdateLoadingProgress(float newProgress) => OnUpdateLoadingProgress?.Invoke(newProgress);
    public static event Action<float> OnUpdateLoadingProgress;

}