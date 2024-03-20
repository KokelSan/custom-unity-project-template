using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingService : Service
{
    private LoadingReport _loadingReport;
    private SceneLoadingParameters _pendingLoading;
    
    #region Overrides

    protected override void EventHandlerRegister()
    {
        SceneLoadingServiceHandlerData.OnLoadScene += LoadScene;
        SceneLoadingServiceHandlerData.OnUnLoadScene += UnLoadScene;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    protected override void EventHandlerUnRegister()
    {
        SceneLoadingServiceHandlerData.OnLoadScene -= LoadScene;
        SceneLoadingServiceHandlerData.OnUnLoadScene -= UnLoadScene;
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        
        _loadingReport.Start();
        _pendingLoading = parameters;

        while (!loadingOperation.isDone)
        {
            if (parameters.HasLoadingScreen)
            {
                float progress = Mathf.Clamp01(loadingOperation.progress);
                LoadingScreenHandlerData.UpdateLoadingProgress(progress);
            }
            yield return null;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if(scene.buildIndex == 0) return; // We ignore the BootScene, which is loaded directly from Unity at start
        
        if (_pendingLoading.Equals(default))
        {
            Debug.LogWarning($"Scene {scene.buildIndex} has been loaded outside the SceneLoadingService");
            return;
        }

        if (scene.buildIndex != _pendingLoading.SceneIndex)
        {
            Debug.LogWarning($"The loaded scene {scene.buildIndex} doesn't match with the pending loading of the SceneLoadingService");
            return;
        }
        
        _loadingReport.Finish();
        SceneLoadingServiceHandlerData.SceneLoaded(scene.buildIndex, _loadingReport);

        if (_pendingLoading.HasLoadingScreen)
        {
            LoadingScreenHandlerData.WaitForInput(DeclareSceneReady);
            return;
        }
        
        DeclareSceneReady();
    }

    private void DeclareSceneReady()
    {
        ScreenTransitionServiceHandlerData.HideScreenTransition(OnTransitionCompleted);
        void OnTransitionCompleted()
        {
            SceneLoadingServiceHandlerData.SceneReadyToPlay(_pendingLoading.SceneIndex);
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