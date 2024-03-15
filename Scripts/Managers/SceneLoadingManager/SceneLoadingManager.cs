using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : Manager
{
    #region Overrides

    protected override void EventHandlerRegister()
    {
        SceneLoadingManagerHandlerData.OnLoadScene += VerifySceneLoading;
        SceneLoadingManagerHandlerData.OnUnLoadScene += UnLoadScene;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }

    protected override void EventHandlerUnRegister()
    {
        SceneLoadingManagerHandlerData.OnLoadScene -= VerifySceneLoading;
        SceneLoadingManagerHandlerData.OnUnLoadScene -= UnLoadScene;
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

    private void VerifySceneLoading(int sceneIndex, LoadSceneMode loadSceneMode, TransitionType transitionType, bool isBoot)
    {
        if (!CanSceneBeLoaded(sceneIndex, out string errorMsg))
        {
            Debug.LogWarning($"Scene {sceneIndex} cannot be loaded ({errorMsg}), operation aborted.");
            return;
        }
        
        StartCoroutine(InitiateSceneLoading(sceneIndex, loadSceneMode, transitionType, isBoot));
    }
    
    private IEnumerator InitiateSceneLoading(int sceneIndex, LoadSceneMode loadSceneMode, TransitionType transitionType, bool isBoot)
    {
        if (transitionType != TransitionType.None)
        {
            void OnTransitionCompleted()
            {
                Debug.LogError("Transition completed on SceneLoadingManager");
                StartCoroutine(PerformSceneLoading(sceneIndex,  loadSceneMode,  transitionType));
                ScreenTransitionManagerHandlerData.OnTransitionCompleted -= OnTransitionCompleted;
            }
            
            ScreenTransitionManagerHandlerData.OnTransitionCompleted += OnTransitionCompleted;
            ScreenTransitionManagerHandlerData.ShowTransition(transitionType, isBoot);
            yield break;
        }

        StartCoroutine(PerformSceneLoading(sceneIndex,  loadSceneMode,  transitionType));
    }

    IEnumerator PerformSceneLoading(int sceneIndex, LoadSceneMode loadSceneMode, TransitionType transitionType)
    {
        Debug.LogError("Performing loading");
        
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        if (loadingOperation == null)
        {
            Debug.LogWarning($"Scene {sceneIndex} couldn't be loaded, operation aborted.");
            yield break;
        }
        
        loadingOperation.allowSceneActivation = false;
        while (!loadingOperation.isDone)
        {
            // float progress = loadingOperation.progress / 0.9f * 100;
            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
            }
            yield return null;
        }
        Debug.LogError("Loading completed");

        if (transitionType != TransitionType.None)
        {
            Debug.LogError("Hiding transition");
            ScreenTransitionManagerHandlerData.HideTransition(transitionType);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        SceneLoadingManagerHandlerData.NotifySceneLoaded(scene.buildIndex);
    }
    
    private void OnSceneUnLoaded(Scene scene)
    {
        SceneLoadingManagerHandlerData.NotifySceneUnLoaded(scene.buildIndex);
    }

    #endregion
    
}