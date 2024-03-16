using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : Manager
{
    private TransitionType _pendingTransition;
    private float _loadingStartingTime;
    private int _loadingStartingFrames;
    private float _loadingScreenClickedTime;
    private float _loadingScreenClickedFrames;
    
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
            ScreenTransitionManagerHandlerData.ShowTransition(transitionType, isBoot);
            ScreenTransitionManagerHandlerData.OnTransitionCompleted += OnTransitionCompleted;
            void OnTransitionCompleted()
            {
                ScreenTransitionManagerHandlerData.OnTransitionCompleted -= OnTransitionCompleted;
                StartCoroutine(PerformSceneLoading(sceneIndex,  loadSceneMode,  transitionType, isBoot));
            }
            yield break;
        }
        StartCoroutine(PerformSceneLoading(sceneIndex,  loadSceneMode,  transitionType, isBoot));
    }

    IEnumerator PerformSceneLoading(int sceneIndex, LoadSceneMode loadSceneMode, TransitionType transitionType, bool isBoot)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        if (loadingOperation == null)
        {
            Debug.LogWarning($"Scene {sceneIndex} couldn't be loaded, operation aborted.");
            yield break;
        }

        _pendingTransition = transitionType;
        _loadingStartingTime = Time.time;
        _loadingStartingFrames = Time.frameCount;
        
        bool hasTransition = transitionType != TransitionType.None;
        bool hasLoadingScreen = !isBoot && hasTransition;
        
        if (hasLoadingScreen)
        {
            loadingOperation.allowSceneActivation = false;
            ScreenTransitionManagerHandlerData.OnLoadingScreenClicked += OnLoadingScreenClicked;
            void OnLoadingScreenClicked()
            {
                _loadingScreenClickedTime = Time.time;
                _loadingScreenClickedFrames = Time.frameCount;
                ScreenTransitionManagerHandlerData.OnLoadingScreenClicked -= OnLoadingScreenClicked;
                loadingOperation.allowSceneActivation = true;
            }
        }

        while (!loadingOperation.isDone)
        {
            if (hasLoadingScreen)
            {
                if (loadingOperation.progress >= 0.9f)
                {
                    Debug.Log($"Loading ready after {Time.frameCount - _loadingStartingFrames} Frames (= {Time.time - _loadingStartingTime}s)");
                    ScreenTransitionManagerHandlerData.ShowTransition(TransitionType.LoadingScreen);
                    hasLoadingScreen = false; // So we won't enter this condition anymore
                }
                else
                {
                    SceneLoadingManagerHandlerData.UpdateLoadingProgress(loadingOperation.progress / 0.9f * 100);
                }
            }
            yield return null;
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
        // Debug.Log($"Loading completed in {Time.frameCount - _loadingScreenClickedFrames} Frames (= {Time.time - _loadingScreenClickedTime}s) after loading screen clicked");

        if (_pendingTransition != default)
        {
            ScreenTransitionManagerHandlerData.HideTransition(_pendingTransition);
            ScreenTransitionManagerHandlerData.HideTransition(TransitionType.LoadingScreen);
            _pendingTransition = default;
        }
        
        SceneLoadingManagerHandlerData.SceneLoaded(scene.buildIndex);
    }
    
    private void OnSceneUnLoaded(Scene scene)
    {
        SceneLoadingManagerHandlerData.SceneUnLoaded(scene.buildIndex);
    }

    #endregion
    
}