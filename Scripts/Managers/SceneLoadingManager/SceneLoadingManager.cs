using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct FrameTimePair
{
    public int Frames;
    public float Seconds;
    
    public bool IsNull => Frames == 0 && Seconds == 0;
    public float Rate => Frames / Seconds;

    public void Set()
    {
        Frames = Time.renderedFrameCount;
        Seconds = Time.realtimeSinceStartup;
    }
    
    public void Reset()
    {
        Frames = 0;
        Seconds = 0;
    }

    public static FrameTimePair operator-(FrameTimePair a, FrameTimePair b)
    {
        // Debug.Log($"Pair operation: ({a.Frames}, {a.Seconds}) - ({b.Frames}, {b.Seconds})");
        
        return new FrameTimePair
        {
            Frames = a.Frames - b.Frames,
            Seconds = a.Seconds - b.Seconds
        };
    }
}

public struct LoadingReport
{
    public FrameTimePair Start;
    public FrameTimePair End;
    
    public FrameTimePair Duration => End - Start;
    
    public void Reset()
    {
        Start.Reset();
        End.Reset();
    }
}

public class SceneLoadingManager : Manager
{
    private TransitionType _pendingTransition;
    private LoadingReport _loadingReport;
    
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
        
        _loadingReport.Reset();
        _loadingReport.Start.Set();
        
        bool hasTransition = transitionType != TransitionType.None;
        bool hasLoadingScreen = !isBoot && hasTransition;
        
        if (hasLoadingScreen)
        {
            loadingOperation.allowSceneActivation = false;
            ScreenTransitionManagerHandlerData.OnLoadingScreenClicked += OnLoadingScreenClicked;
            void OnLoadingScreenClicked()
            {
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
                    
                    _loadingReport.End.Set();
                    SceneLoadingManagerHandlerData.SceneReady(sceneIndex, _loadingReport);
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
        if (_pendingTransition != default)
        {
            ScreenTransitionManagerHandlerData.HideTransition(_pendingTransition);
            ScreenTransitionManagerHandlerData.HideTransition(TransitionType.LoadingScreen);
            _pendingTransition = default;
        }

        if (_loadingReport.End.IsNull)
        {
            _loadingReport.End.Set();
            SceneLoadingManagerHandlerData.SceneLoaded(scene.buildIndex, _loadingReport);
            return;
        }
        
        SceneLoadingManagerHandlerData.SceneLoaded(scene.buildIndex);
    }
    
    private void OnSceneUnLoaded(Scene scene)
    {
        SceneLoadingManagerHandlerData.SceneUnLoaded(scene.buildIndex);
    }

    #endregion
    
}