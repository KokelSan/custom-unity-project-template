using UnityEditor;
using UnityEngine;

public class GameManager : BaseBehaviour
{
    public StartConfigSO StartConfig;
    
    private bool _isGameStarted = false;
    private bool _isGamePaused = false;
    
    #region Overrides

        protected override void EventHandlerRegister()
        {
            SceneLoadingServiceHandlerData.OnSceneReadyToPlay += OnSceneReadyToPlay;
            
            GameManagerHandlerData.OnStartGame += StartGame;
            GameManagerHandlerData.OnStopGame += StopGame;
            GameManagerHandlerData.OnPauseGame += PauseGame;
            GameManagerHandlerData.OnResumeGame += ResumeGame;
            GameManagerHandlerData.OnIsGamePaused += IsGamePaused;
            GameManagerHandlerData.OnExitGame += ExitGame;

            InputManagerHandlerData.OnEscape += TogglePause;
        }
    
        protected override void EventHandlerUnRegister()
        {
            SceneLoadingServiceHandlerData.OnSceneReadyToPlay -= OnSceneReadyToPlay;
            
            GameManagerHandlerData.OnStartGame -= StartGame;
            GameManagerHandlerData.OnStopGame -= StopGame;
            GameManagerHandlerData.OnPauseGame -= PauseGame;            
            GameManagerHandlerData.OnResumeGame -= ResumeGame;
            GameManagerHandlerData.OnIsGamePaused -= IsGamePaused;
            GameManagerHandlerData.OnExitGame -= ExitGame;
            
            InputManagerHandlerData.OnEscape -= TogglePause;
        }

    #endregion

    private void StartGame()
    {
        if(_isGameStarted) return;

        if (StartConfig.LoadSceneOnStartGame)
        {
            SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingData(StartConfig.SceneIndexToLoadOnStart));
            return;
        }
        
        _isGameStarted = true;
        GameManagerHandlerData.GameStarted();
    }

    private void OnSceneReadyToPlay(int sceneIndex)
    {
        // Main menu scene
        if(sceneIndex == StartConfig.MainMenuSceneIndex)
        {
            UIMenuManagerHandlerData.ShowMainMenu();
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
        
        Time.timeScale = 1;
        _isGameStarted = _isGamePaused = false;
        GameManagerHandlerData.GameStopped();
        
        if (SceneLoadingServiceHandlerData.GetActiveScene() == StartConfig.MainMenuSceneIndex)
        {
            UIMenuManagerHandlerData.ShowMainMenu();
        }
        else
        {
            SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingData(StartConfig.MainMenuSceneIndex));
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