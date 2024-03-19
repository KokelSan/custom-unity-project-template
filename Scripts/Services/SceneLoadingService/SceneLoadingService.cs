using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingService : Service
{
    private LoadingReport _loadingReport;
    
    #region Overrides

    protected override void EventHandlerRegister()
    {
        SceneLoadingServiceHandlerData.OnLoadScene += LoadScene;
        SceneLoadingServiceHandlerData.OnUnLoadScene += UnLoadScene;
        // SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    protected override void EventHandlerUnRegister()
    {
        SceneLoadingServiceHandlerData.OnLoadScene -= LoadScene;
        SceneLoadingServiceHandlerData.OnUnLoadScene -= UnLoadScene;
        // SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }

    #endregion

    #region Utils

    private bool IsSceneIndexValid(int sceneIndex)
    {
        return sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings;
    }
    
    private bool IsSceneLoaded(int sceneIndex)
    {
        return SceneManager.GetSceneByBuildIndex(sceneIndex).IsValid();
    }
    
    private bool IsSceneLastLoadedScene(int sceneIndex)
    {
        return SceneManager.loadedSceneCount == 1 && SceneManager.GetActiveScene().buildIndex == sceneIndex;
    }

    private bool CanSceneBeLoaded(int sceneIndex, out string errorMsg)
    {
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
    
    private bool CanSceneBeUnLoaded(int sceneIndex, out string errorMsg)
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

    private void LoadScene(SceneLoadingParameters parameters)
    {
        if (!CanSceneBeLoaded(parameters.SceneIndex, out string errorMsg))
        {
            Debug.LogWarning($"Scene {parameters.SceneIndex} cannot be loaded ({errorMsg}), operation aborted.");
            return;
        }
        
        ScreenTransitionServiceHandlerData.ShowScreenTransition(parameters.ScreenTransitionType, OnTransitionCompleted);
        void OnTransitionCompleted()
        {
            StartCoroutine(LoadSceneAsync(parameters));
        }
    }

    IEnumerator LoadSceneAsync(SceneLoadingParameters parameters)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(parameters.SceneIndex, parameters.LoadSceneMode);
        if (loadingOperation == null)
        {
            Debug.LogWarning($"Scene {parameters.SceneIndex} couldn't be loaded, operation aborted.");
            yield break;
        }
        
        Debug.Log("Loading started");
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        _loadingReport.Start();

        while (!loadingOperation.isDone)
        {
            if (parameters.ScreenTransitionType == ScreenTransitionType.LoadingScreen)
            {
                float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
                LoadingScreenHandlerData.UpdateLoadingProgress(progress);
            }
            yield return null;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        _loadingReport.EndTime.Set();
        SceneLoadingServiceHandlerData.SceneLoaded(scene.buildIndex, _loadingReport);
        
        ScreenTransitionServiceHandlerData.HideScreenTransition(OnTransitionCompleted);
        void OnTransitionCompleted()
        {
            SceneLoadingServiceHandlerData.SceneReadyToPlay(scene.buildIndex);
        }
    }
    
    private void UnLoadScene(int sceneIndex)
    {
        if (!CanSceneBeUnLoaded(sceneIndex, out string errorMsg))
        {
            Debug.LogWarning($"Scene {sceneIndex} cannot be unloaded ({errorMsg}), operation aborted.");
            return;
        }

        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    
    private void OnSceneUnLoaded(Scene scene)
    {
        SceneLoadingServiceHandlerData.SceneUnLoaded(scene.buildIndex);
    }

    #endregion
    
}