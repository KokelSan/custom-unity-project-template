using UnityEditor;
using UnityEngine;

public class GameManager : Manager
{
    private bool _isGameStarted = false;
    private bool _isGamePaused = false;
    
    #region Overrides

        protected override void EventHandlerRegister()
        {
            GameManagerManagerHandlerData.OnStartGame += StartGame;
            GameManagerManagerHandlerData.OnPauseGame += PauseGame;
            GameManagerManagerHandlerData.OnResumeGame += ResumeGame;
            GameManagerManagerHandlerData.OnTogglePause += TogglePause;
            GameManagerManagerHandlerData.OnStopGame += StopGame;
            GameManagerManagerHandlerData.OnExitGame += ExitGame;

            InputManagerHandlerData.OnEscape += TogglePause;
        }
    
        protected override void EventHandlerUnRegister()
        {
            GameManagerManagerHandlerData.OnStartGame -= StartGame;
            GameManagerManagerHandlerData.OnPauseGame -= PauseGame;            
            GameManagerManagerHandlerData.OnResumeGame -= ResumeGame;
            GameManagerManagerHandlerData.OnTogglePause -= TogglePause;
            GameManagerManagerHandlerData.OnStopGame -= StopGame;
            GameManagerManagerHandlerData.OnExitGame -= ExitGame;
            
            InputManagerHandlerData.OnEscape -= TogglePause;
        }

    #endregion

    private void StartGame()
    {
        if(_isGameStarted) return;
        _isGameStarted = true;
        GameManagerManagerHandlerData.GameStarted();
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
            GameManagerManagerHandlerData.GamePaused();
        }
        else
        {
            Time.timeScale = 1;
            GameManagerManagerHandlerData.GameResumed();
        }
    }

    private void StopGame()
    {
        if(!_isGameStarted) return;
        _isGameStarted = _isGamePaused = false;
        Time.timeScale = 1;
        GameManagerManagerHandlerData.GameStopped();
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