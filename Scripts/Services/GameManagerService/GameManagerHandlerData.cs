using System;

public static class GameManagerHandlerData
{
    public static void StartGame() => OnStartGame?.Invoke();
    public static event Action OnStartGame;
    public static void GameStarted() => OnGameStarted?.Invoke();
    public static event Action OnGameStarted;
    
    
    public static void PauseGame() => OnPauseGame?.Invoke();
    public static event Action OnPauseGame;
    public static void GamePaused() => OnGamePaused?.Invoke();
    public static event Action OnGamePaused;
    public static bool IsGamePaused() => OnIsGamePaused?.Invoke() ?? false;
    public static event Func<bool> OnIsGamePaused; 
    
    
    public static void ResumeGame() => OnResumeGame?.Invoke();
    public static event Action OnResumeGame;
    public static void GameResumed() => OnGameResumed?.Invoke();
    public static event Action OnGameResumed;
    
    
    public static void StopGame() => OnStopGame?.Invoke();
    public static event Action OnStopGame;
    public static void GameStopped() => OnGameStopped?.Invoke();
    public static event Action OnGameStopped;
    
    
    public static void ExitGame() => OnExitGame?.Invoke();
    public static event Action OnExitGame;
    
}