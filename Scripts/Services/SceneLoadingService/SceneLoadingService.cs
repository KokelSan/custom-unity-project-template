using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingService : Service
{
    private SceneLoadingData _pendingLoading;
    
    public bool IsLoading => _pendingLoading != null;
    
    #region Overrides

    protected override void EventHandlerRegister()
    {
        SceneLoadingServiceHandlerData.OnLoadScene += LoadScene;
        SceneLoadingServiceHandlerData.OnUnLoadScene += UnLoadScene;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
        SceneLoadingServiceHandlerData.OnGetActiveScene += GetActiveScene;
    }

    protected override void EventHandlerUnRegister()
    {
        SceneLoadingServiceHandlerData.OnLoadScene -= LoadScene;
        SceneLoadingServiceHandlerData.OnUnLoadScene -= UnLoadScene;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
        SceneLoadingServiceHandlerData.OnGetActiveScene -= GetActiveScene;
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

    private void LoadScene(SceneLoadingData loadingData)
    {
        if (!CanSceneBeLoaded(loadingData.SceneToLoadIndex, out string errorMsg))
        {
            Debug.LogWarning($"Scene {loadingData.SceneToLoadIndex} cannot be loaded ({errorMsg}), operation aborted.");
            return;
        }
        
        _pendingLoading = loadingData;
        
        UITransitionServiceHandlerData.ShowSceneTransition(GetActiveScene(), loadingData.SceneToLoadIndex, OnAnimationCompleted);
        void OnAnimationCompleted()
        {
            StartCoroutine(LoadSceneAsync(loadingData));
        }
    }

    IEnumerator LoadSceneAsync(SceneLoadingData loadingData)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(loadingData.SceneToLoadIndex, loadingData.LoadSceneMode);
        if (loadingOperation == null)
        {
            Debug.LogWarning($"Scene {loadingData.SceneToLoadIndex} couldn't be loaded, operation aborted.");
            _pendingLoading = null;
            yield break;
        }

        
        _pendingLoading.StartDurationRecord();

        while (!loadingOperation.isDone)
        {
            UILoadingScreenHandlerData.UpdateLoadingProgress(loadingOperation.progress);
            yield return null;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if(scene.buildIndex == 0) return; // We ignore the BootScene, which is loaded directly from Unity at start

        if (scene.buildIndex != _pendingLoading.SceneToLoadIndex)
        {
            Debug.LogWarning($"The loaded scene ({scene.buildIndex}) doesn't match with the expected pending one ({_pendingLoading.SceneToLoadIndex}");
            return;
        }
        
        _pendingLoading.StopDurationRecord();
        SceneLoadingServiceHandlerData.SceneLoaded(scene.buildIndex, _pendingLoading);

        if (UILoadingScreenHandlerData.ShouldWaitForInput())
        {
            UILoadingScreenHandlerData.WaitForInput(DeclareSceneReady);
            return;
        }
        
        DeclareSceneReady();
    }

    private void DeclareSceneReady()
    {
        UITransitionServiceHandlerData.HideTransition(OnAnimationCompleted);
        void OnAnimationCompleted()
        {
            SceneLoadingServiceHandlerData.SceneReadyToPlay(_pendingLoading.SceneToLoadIndex);
            _pendingLoading = null;
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

    private int GetActiveScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    #endregion
    
}