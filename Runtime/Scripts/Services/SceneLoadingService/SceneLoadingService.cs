using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoadingService
{
    public static bool IsLoading => _pendingLoading != null;
    private static SceneLoadingData _pendingLoading;
    
    public static int ActiveScene => SceneManager.GetActiveScene().buildIndex;

    public static event Action<AsyncOperation> OnLoadingStarted; 
    public static event Action<int, float> OnSceneLoaded; // sceneIndex, loadingDuration
    public static event Action<int> OnSceneUnLoaded;
    public static event Action<int> OnSceneReadyToPlay;


    #region Utils

    private static bool IsSceneIndexValid(int sceneIndex)
    {
        return sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings;
    }
    
    private static bool IsSceneLoaded(int sceneIndex)
    {
        return SceneManager.GetSceneByBuildIndex(sceneIndex).IsValid();
    }
    
    private static bool IsSceneLastLoadedScene(int sceneIndex)
    {
        return SceneManager.loadedSceneCount == 1 && SceneManager.GetActiveScene().buildIndex == sceneIndex;
    }

    private static bool CanSceneBeLoaded(int sceneIndex, out string errorMsg)
    {
        if (IsLoading)
        {
            errorMsg = _pendingLoading.SceneToLoadIndex == sceneIndex ? "already loading this scene" : "already loading another scene";
            return false;
        }
        
        if (!IsSceneIndexValid(sceneIndex))
        {
            errorMsg = "invalid index";
            return false;
        }
        
        if (IsSceneLoaded(sceneIndex))
        {
            errorMsg = "scene already loaded";
            return false;
        }

        errorMsg = "";
        return true;
    }
    
    private static bool CanSceneBeUnLoaded(int sceneIndex, out string errorMsg)
    {
        if (!IsSceneIndexValid(sceneIndex))
        {
            errorMsg = "invalid index";
            return false;
        }
        
        if (!IsSceneLoaded(sceneIndex))
        {
            errorMsg = "scene not loaded";
            return false;
        }
        
        if (IsSceneLastLoadedScene(sceneIndex))
        {
            errorMsg = "last loaded scene";
            return false;
        }
        
        errorMsg = "";
        return true;
    }

    #endregion

    #region Load/Unload Core

    public static void LoadScene(SceneLoadingData loadingData)
    {
        if (!CanSceneBeLoaded(loadingData.SceneToLoadIndex, out string errorMsg))
        {
            Debug.LogWarning($"Scene {loadingData.SceneToLoadIndex} cannot be loaded ({errorMsg}), operation aborted.");
            return;
        }
        
        _pendingLoading = loadingData;
        
        UITransitionManagerHandlerData.ShowSceneTransition(ActiveScene, loadingData.SceneToLoadIndex, OnAnimationCompleted);
        void OnAnimationCompleted()
        {
            PerformLoading(loadingData);
        }
    }

    private static void PerformLoading(SceneLoadingData loadingData)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(loadingData.SceneToLoadIndex, loadingData.LoadSceneMode);
        if (loadingOperation == null)
        {
            Debug.LogWarning($"Scene {loadingData.SceneToLoadIndex} couldn't be loaded, operation aborted.");
            _pendingLoading = null;
            return;
        }
        
        _pendingLoading.StartDurationRecord();
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnLoaded;
        
        OnLoadingStarted?.Invoke(loadingOperation);
    }
    
    private static void SceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.buildIndex != _pendingLoading.SceneToLoadIndex)
        {
            Debug.LogWarning($"The loaded scene ({scene.buildIndex}) doesn't match with the expected pending one ({_pendingLoading.SceneToLoadIndex}");
            return;
        }
        
        _pendingLoading.StopDurationRecord();
        SceneManager.sceneLoaded -= SceneLoaded;
        OnSceneLoaded?.Invoke(scene.buildIndex, _pendingLoading.Duration);
    }

    public static void CompleteLoading()
    {
        UITransitionManagerHandlerData.HideTransition(OnAnimationCompleted);
        void OnAnimationCompleted()
        {
            OnSceneReadyToPlay?.Invoke(_pendingLoading.SceneToLoadIndex);
            _pendingLoading = null;
        }
    }
    
    public static void UnLoadScene(int sceneIndex)
    {
        if (!CanSceneBeUnLoaded(sceneIndex, out string errorMsg))
        {
            Debug.LogWarning($"Scene {sceneIndex} cannot be unloaded ({errorMsg}), operation aborted.");
            return;
        }

        SceneManager.sceneUnloaded += SceneUnLoaded;
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    
    private static void SceneUnLoaded(Scene scene)
    {
        SceneManager.sceneUnloaded -= SceneUnLoaded;
        OnSceneUnLoaded?.Invoke(scene.buildIndex);
    }

    #endregion
    
}