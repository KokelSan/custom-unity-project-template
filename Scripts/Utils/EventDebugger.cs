using UnityEngine;

public class EventDebugger : BaseBehaviour
{
    public bool DebugSceneLoading;
    private bool _sceneLoadingDebugged;
    
    public bool DebugInputs;
    private bool _inputsDebugged;
    
    public bool DebugGameStates;
    private bool _gameStatesDebugged;

    protected override void Initialize()
    {
        base.Initialize();
        DontDestroyOnLoad(this);
    }

    private void OnValidate()
    {
        SetSceneLoadingDebug();
        SetInputsDebug();
        SetDebugGameStates();
    }

    #region Scene Loading

    private void SetSceneLoadingDebug()
    {
        if (DebugSceneLoading && !_sceneLoadingDebugged)
        {
            _sceneLoadingDebugged = true;
            SceneLoadingManagerHandlerData.OnSceneLoaded += OnSceneLoaded;
            SceneLoadingManagerHandlerData.OnSceneReady += OnSceneReady;
            SceneLoadingManagerHandlerData.OnSceneUnLoaded += OnSceneUnLoaded;
        }
        
        if (!DebugSceneLoading && _sceneLoadingDebugged)
        {
            _sceneLoadingDebugged = false;
            SceneLoadingManagerHandlerData.OnSceneLoaded -= OnSceneLoaded;
            SceneLoadingManagerHandlerData.OnSceneReady -= OnSceneReady;
            SceneLoadingManagerHandlerData.OnSceneUnLoaded -= OnSceneUnLoaded;
        }
    }
    
    private void OnSceneLoaded(int index, LoadingReport report) => Debug.Log($"Scene {index} successfully loaded {(report.Equals(default(LoadingReport)) ? "" : $"in {report.Duration}s")}");
    private void OnSceneReady(int index, LoadingReport report) => Debug.Log($"Scene {index} ready after {report.Duration}s");
    private void OnSceneUnLoaded(int index) => Debug.Log($"Scene {index} successfully unloaded");

    #endregion

    #region Inputs

    private void SetInputsDebug()
    {
        if (DebugInputs && !_inputsDebugged)
        {
            _inputsDebugged = true;
            InputManagerHandlerData.OnMove += OnMove;
            InputManagerHandlerData.OnLook += OnLook;
            InputManagerHandlerData.OnRightClick += OnRightClick;
            InputManagerHandlerData.OnEscape += OnEscape;
            InputManagerHandlerData.OnSpace += OnSpace;
            InputManagerHandlerData.OnTap += OnTap;
            InputManagerHandlerData.OnPointerMove += OnPointerMove;
            InputManagerHandlerData.OnClick += OnClick;
        }
        
        if (!DebugInputs && _inputsDebugged)
        {
            _inputsDebugged = false;
            InputManagerHandlerData.OnMove -= OnMove;
            InputManagerHandlerData.OnLook -= OnLook;
            InputManagerHandlerData.OnRightClick -= OnRightClick;
            InputManagerHandlerData.OnEscape -= OnEscape;
            InputManagerHandlerData.OnSpace -= OnSpace;
            InputManagerHandlerData.OnTap -= OnTap;
            InputManagerHandlerData.OnPointerMove -= OnPointerMove;
            InputManagerHandlerData.OnClick -= OnClick;
        }
    }
    
    private void OnMove(Vector2 _) => Debug.Log("Moving");
    private void OnLook(Vector2 _) => Debug.Log("Looking");
    private void OnRightClick(Vector2 pos) => Debug.Log($"Right click at {pos}");
    private void OnEscape() => Debug.Log("Escape pressed");
    private void OnSpace() => Debug.Log("Space pressed");
    private void OnTap(Vector2 pos) => Debug.Log($"Tap at {pos}");
    private void OnPointerMove(Vector2 pos) => Debug.Log("Pointer moving");
    private void OnClick(Vector2 pos) => Debug.Log($"Click at {pos}");

    #endregion

    #region Game states

    private void SetDebugGameStates()
    {
        if (DebugGameStates && !_gameStatesDebugged)
        {
            _gameStatesDebugged = true;
            GameManagerManagerHandlerData.OnGameStarted += OnGameStarted;
            GameManagerManagerHandlerData.OnGamePaused += OnGamePaused;
            GameManagerManagerHandlerData.OnGameResumed += OnGameResumed;
            GameManagerManagerHandlerData.OnGameStopped += OnGameStopped;
        }
        
        if (!DebugGameStates && _gameStatesDebugged)
        {
            _gameStatesDebugged = false;
            GameManagerManagerHandlerData.OnGameStarted -= OnGameStarted;
            GameManagerManagerHandlerData.OnGamePaused -= OnGamePaused;
            GameManagerManagerHandlerData.OnGameResumed -= OnGameResumed;
            GameManagerManagerHandlerData.OnGameStopped -= OnGameStopped;
        }
    }
    
    private void OnGameStarted() => Debug.Log("Game started");
    private void OnGamePaused() => Debug.Log("Game paused");
    private void OnGameResumed() => Debug.Log("Game resumed");
    private void OnGameStopped() => Debug.Log("Game stopped");

    #endregion
    
}