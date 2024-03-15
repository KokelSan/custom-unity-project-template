using UnityEngine;

public class EventDebugger : Manager
{
    public bool DebugSceneLoading;
    public bool DebugInputs;
    public bool DebugGameStates;
    public bool DebugScreenTransitions;
    
    protected override void EventHandlerRegister()
    {
        if (DebugSceneLoading)
        {
            SceneLoadingManagerHandlerData.OnSceneLoaded += OnSceneLoaded;
            SceneLoadingManagerHandlerData.OnSceneUnLoaded += OnSceneUnLoaded;
        }

        if (DebugInputs)
        {
            InputManagerHandlerData.OnMove += OnMove;
            InputManagerHandlerData.OnLook += OnLook;
            InputManagerHandlerData.OnRightClick += OnRightClick;
            InputManagerHandlerData.OnEscape += OnEscape;
            InputManagerHandlerData.OnSpace += OnSpace;
            InputManagerHandlerData.OnTap += OnTap;
            InputManagerHandlerData.OnPointerMove += OnPointerMove;
            InputManagerHandlerData.OnClick += OnClick;
        }

        if (DebugGameStates)
        {
            GameManagerManagerHandlerData.OnGameStarted += OnGameStarted;
            GameManagerManagerHandlerData.OnGamePaused += OnGamePaused;
            GameManagerManagerHandlerData.OnGameResumed += OnGameResumed;
            GameManagerManagerHandlerData.OnGameStopped += OnGameStopped;
        }

        if (DebugScreenTransitions)
        {
            ScreenTransitionManagerHandlerData.OnTransitionStarted += OnTransitionStarted;
            ScreenTransitionManagerHandlerData.OnTransitionCompleted += OnTransitionCompleted;
        }
    }
    
    protected override void EventHandlerUnRegister()
    {
        if (DebugSceneLoading)
        {
            SceneLoadingManagerHandlerData.OnSceneLoaded -= OnSceneLoaded;
            SceneLoadingManagerHandlerData.OnSceneUnLoaded -= OnSceneUnLoaded;
        }

        if (DebugInputs)
        {
            InputManagerHandlerData.OnMove -= OnMove;
            InputManagerHandlerData.OnLook -= OnLook;
            InputManagerHandlerData.OnRightClick -= OnRightClick;
            InputManagerHandlerData.OnEscape -= OnEscape;
            InputManagerHandlerData.OnSpace -= OnSpace;
            InputManagerHandlerData.OnTap -= OnTap;
            InputManagerHandlerData.OnPointerMove -= OnPointerMove;
            InputManagerHandlerData.OnClick -= OnClick;
        }

        if (DebugGameStates)
        {
            GameManagerManagerHandlerData.OnGameStarted -= OnGameStarted;
            GameManagerManagerHandlerData.OnGamePaused -= OnGamePaused;
            GameManagerManagerHandlerData.OnGameResumed -= OnGameResumed;
            GameManagerManagerHandlerData.OnGameStopped -= OnGameStopped;
        }
        
        if (DebugScreenTransitions)
        {
            ScreenTransitionManagerHandlerData.OnTransitionStarted -= OnTransitionStarted;
            ScreenTransitionManagerHandlerData.OnTransitionCompleted -= OnTransitionCompleted;
        }
    }

    #region Scene Loading

    private void OnSceneLoaded(int index) => Debug.Log($"SceneLoadingManager: Scene {index} successfully loaded");
    private void OnSceneUnLoaded(int index) => Debug.Log($"SceneLoadingManager: Scene {index} successfully unloaded");

    #endregion

    #region Inputs

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

    private void OnGameStarted() => Debug.Log("Game started");
    private void OnGamePaused() => Debug.Log("Game paused");
    private void OnGameResumed() => Debug.Log("Game resumed");
    private void OnGameStopped() => Debug.Log("Game stopped");

    #endregion
    
    #region Screen transitions

    private void OnTransitionStarted() => Debug.Log("Transition started");
    private void OnTransitionCompleted() => Debug.Log("Transition completed");

    #endregion
    
}