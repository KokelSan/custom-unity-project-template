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
            SceneLoadingServiceHandlerData.OnSceneLoaded += OnSceneLoaded;
            SceneLoadingServiceHandlerData.OnSceneReadyToPlay += OnSceneReadyToPlay;
            SceneLoadingServiceHandlerData.OnSceneUnLoaded += OnSceneUnLoaded;
        }
        
        if (!DebugSceneLoading && _sceneLoadingDebugged)
        {
            _sceneLoadingDebugged = false;
            SceneLoadingServiceHandlerData.OnSceneLoaded -= OnSceneLoaded;
            SceneLoadingServiceHandlerData.OnSceneReadyToPlay -= OnSceneReadyToPlay;
            SceneLoadingServiceHandlerData.OnSceneUnLoaded -= OnSceneUnLoaded;
        }
    }
    
    private void OnSceneLoaded(int index, SceneLoadingData data) => Debug.Log($"Scene {index} loaded in {data.Duration}s");
    private void OnSceneReadyToPlay(int index) => Debug.Log($"Scene {index} ready to play");
    private void OnSceneUnLoaded(int index) => Debug.Log($"Scene {index} unloaded");

    #endregion

    #region Inputs

    private void SetInputsDebug()
    {
        if (DebugInputs && !_inputsDebugged)
        {
            _inputsDebugged = true;
            InputServiceHandlerData.OnMove += OnMove;
            InputServiceHandlerData.OnLook += OnLook;
            InputServiceHandlerData.OnRightClick += OnRightClick;
            InputServiceHandlerData.OnEscape += OnEscape;
            InputServiceHandlerData.OnSpace += OnSpace;
            InputServiceHandlerData.OnTap += OnTap;
            InputServiceHandlerData.OnPointerMove += OnPointerMove;
            InputServiceHandlerData.OnClick += OnClick;
        }
        
        if (!DebugInputs && _inputsDebugged)
        {
            _inputsDebugged = false;
            InputServiceHandlerData.OnMove -= OnMove;
            InputServiceHandlerData.OnLook -= OnLook;
            InputServiceHandlerData.OnRightClick -= OnRightClick;
            InputServiceHandlerData.OnEscape -= OnEscape;
            InputServiceHandlerData.OnSpace -= OnSpace;
            InputServiceHandlerData.OnTap -= OnTap;
            InputServiceHandlerData.OnPointerMove -= OnPointerMove;
            InputServiceHandlerData.OnClick -= OnClick;
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
            GameManagerHandlerData.OnGameStarted += OnGameStarted;
            GameManagerHandlerData.OnGamePaused += OnGamePaused;
            GameManagerHandlerData.OnGameResumed += OnGameResumed;
            GameManagerHandlerData.OnGameStopped += OnGameStopped;
        }
        
        if (!DebugGameStates && _gameStatesDebugged)
        {
            _gameStatesDebugged = false;
            GameManagerHandlerData.OnGameStarted -= OnGameStarted;
            GameManagerHandlerData.OnGamePaused -= OnGamePaused;
            GameManagerHandlerData.OnGameResumed -= OnGameResumed;
            GameManagerHandlerData.OnGameStopped -= OnGameStopped;
        }
    }
    
    private void OnGameStarted() => Debug.Log("Game started");
    private void OnGamePaused() => Debug.Log("Game paused");
    private void OnGameResumed() => Debug.Log("Game resumed");
    private void OnGameStopped() => Debug.Log("Game stopped");

    #endregion
    
}