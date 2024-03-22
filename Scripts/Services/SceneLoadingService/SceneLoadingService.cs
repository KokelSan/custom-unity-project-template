using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingService : Service
{
    private bool _isLoading;
    
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
        if (_isLoading)
        {
            errorMsg = _pendingLoading.SceneIndex == sceneIndex ? "already loading this scene" : "already loading another scene";
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
        
        UISceneTransitionServiceHandlerData.ShowScreenTransition(parameters.ScreenTransitionType, OnAnimationCompleted);
        void OnAnimationCompleted()
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

        _isLoading = true;
        _loadingReport.Start();
        _pendingLoading = parameters;

        while (!loadingOperation.isDone)
        {
            if (parameters.HasLoadingScreen)
            {
                float progress = loadingOperation.progress;
                UILoadingScreenHandlerData.UpdateLoadingProgress(progress);
            }
            yield return null;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if(scene.buildIndex == 0) return; // We ignore the BootScene, which is loaded directly from Unity at start

        if (scene.buildIndex != _pendingLoading.SceneIndex)
        {
            Debug.LogWarning($"The loaded scene ({scene.buildIndex}) doesn't match with the pending load operation ({_pendingLoading.SceneIndex}");
            return;
        }
        
        _loadingReport.Stop();
        SceneLoadingServiceHandlerData.SceneLoaded(scene.buildIndex, _loadingReport);

        if (_pendingLoading.ShouldWaitForInputAfterLoading)
        {
            Debug.Log("Wait for input");
            UILoadingScreenHandlerData.WaitForInput(DeclareSceneReady);
            return;
        }
        
        DeclareSceneReady();
    }

    private void DeclareSceneReady()
    {
        UISceneTransitionServiceHandlerData.HideScreenTransition(OnAnimationCompleted);
        void OnAnimationCompleted()
        {
            SceneLoadingServiceHandlerData.SceneReadyToPlay(_pendingLoading.SceneIndex);
            _isLoading = false;
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