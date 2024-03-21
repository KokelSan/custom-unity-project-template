using UnityEditor;
using UnityEngine;

public class GameStateService : Service
{
    private bool _isGameStarted = false;
    private bool _isGamePaused = false;
    
    #region Overrides

        protected override void EventHandlerRegister()
        {
            GameStateServiceHandlerData.OnStartGame += StartGame;
            GameStateServiceHandlerData.OnStopGame += StopGame;
            GameStateServiceHandlerData.OnPauseGame += PauseGame;
            GameStateServiceHandlerData.OnResumeGame += ResumeGame;
            GameStateServiceHandlerData.OnIsGamePaused += IsGamePaused;
            GameStateServiceHandlerData.OnExitGame += ExitGame;

            InputServiceHandlerData.OnEscape += TogglePause;
        }
    
        protected override void EventHandlerUnRegister()
        {
            GameStateServiceHandlerData.OnStartGame -= StartGame;
            GameStateServiceHandlerData.OnStopGame -= StopGame;
            GameStateServiceHandlerData.OnPauseGame -= PauseGame;            
            GameStateServiceHandlerData.OnResumeGame -= ResumeGame;
            GameStateServiceHandlerData.OnIsGamePaused -= IsGamePaused;
            GameStateServiceHandlerData.OnExitGame -= ExitGame;
            
            InputServiceHandlerData.OnEscape -= TogglePause;
        }

    #endregion

    private void StartGame()
    {
        if(_isGameStarted) return;
        _isGameStarted = true;
        GameStateServiceHandlerData.GameStarted();
    }
    
    private void StopGame()
    {
        if(!_isGameStarted) return;
        _isGameStarted = _isGamePaused = false;
        Time.timeScale = 1;
        GameStateServiceHandlerData.GameStopped();
    }

    private void PauseGame()
    {
        if(_isGamePaused) return;
        TogglePause();
    }
    
    private void ResumeGame()
    {
        if(!_isGamePaused) return;
        TogglePause();
    }

    private void TogglePause()
    {
        if(!_isGameStarted) return;
        
        _isGamePaused = !_isGamePaused;

        if (_isGamePaused)
        {
            Time.timeScale = 0;
            GameStateServiceHandlerData.GamePaused();
        }
        else
        {
            Time.timeScale = 1;
            GameStateServiceHandlerData.GameResumed();
        }
    }

    private bool IsGamePaused()
    {
        return _isGamePaused;
    }

    private void ExitGame()
    {
        
#if UNITY_STANDALONE
        Application.Quit();
        Debug.Log("Application closed");
#endif

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        
    }
}