using UnityEditor;
using UnityEngine;

public class GameManager : Service
{
    private bool _isGameStarted = false;
    private bool _isGamePaused = false;
    
    #region Overrides

        protected override void EventHandlerRegister()
        {
            SceneLoadingServiceHandlerData.OnSceneLoaded += OnSceneLoaded;
            
            GameManagerHandlerData.OnStartGame += StartGame;
            GameManagerHandlerData.OnStopGame += StopGame;
            GameManagerHandlerData.OnPauseGame += PauseGame;
            GameManagerHandlerData.OnResumeGame += ResumeGame;
            GameManagerHandlerData.OnIsGamePaused += IsGamePaused;
            GameManagerHandlerData.OnExitGame += ExitGame;

            InputServiceHandlerData.OnEscape += TogglePause;
        }
    
        protected override void EventHandlerUnRegister()
        {
            SceneLoadingServiceHandlerData.OnSceneLoaded -= OnSceneLoaded;
            
            GameManagerHandlerData.OnStartGame -= StartGame;
            GameManagerHandlerData.OnStopGame -= StopGame;
            GameManagerHandlerData.OnPauseGame -= PauseGame;            
            GameManagerHandlerData.OnResumeGame -= ResumeGame;
            GameManagerHandlerData.OnIsGamePaused -= IsGamePaused;
            GameManagerHandlerData.OnExitGame -= ExitGame;
            
            InputServiceHandlerData.OnEscape -= TogglePause;
        }

    #endregion

    private void StartGame()
    {
        if(_isGameStarted) return;
        SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(2));
    }

    private void OnSceneLoaded(int sceneIndex, LoadingReport _)
    {
        // Main menu scene
        if(sceneIndex == 1)
        {
            UIMainMenuHandlerData.ShowMenu();
        }
        
        // First scene
        else if(sceneIndex == 2)
        {
            if(_isGameStarted) return;
            _isGameStarted = true;
            GameManagerHandlerData.GameStarted();
        }
    }
    
    private void StopGame()
    {
        if(!_isGameStarted) return;
        ResumeGame();
        _isGameStarted = false;
        GameManagerHandlerData.GameStopped();
        SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(1));
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
            GameManagerHandlerData.GamePaused();
        }
        else
        {
            Time.timeScale = 1;
            GameManagerHandlerData.GameResumed();
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