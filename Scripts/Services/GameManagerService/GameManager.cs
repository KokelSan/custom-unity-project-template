using UnityEditor;
using UnityEngine;

public class GameManager : Service
{
    public StartConfigSO StartConfig;
    
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

        if (StartConfig.LoadSceneOnStartGame)
        {
            SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(StartConfig.SceneIndexToLoadOnStart));
            return;
        }
        
        _isGameStarted = true;
        GameManagerHandlerData.GameStarted();
    }

    private void OnSceneLoaded(int sceneIndex, LoadingReport _)
    {
        // Main menu scene
        if(sceneIndex == StartConfig.MainMenuSceneIndex)
        {
            UIMainMenuHandlerData.ShowMenu();
        }
        
        // Game scene
        else if(StartConfig.LoadSceneOnStartGame && sceneIndex == StartConfig.SceneIndexToLoadOnStart)
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
        
        if (SceneLoadingServiceHandlerData.GetActiveScene() == StartConfig.MainMenuSceneIndex)
        {
            UIMainMenuHandlerData.ShowMenu();
        }
        else
        {
            SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(1));
        }
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