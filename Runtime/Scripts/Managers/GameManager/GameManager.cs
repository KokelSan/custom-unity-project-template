﻿using UnityEditor;
using UnityEngine;

public class GameManager : BaseBehaviour
{
    public StartConfigSO StartConfig;
    
    private bool _isGameStarted = false;
    private bool _isGamePaused = false;
    
    #region Overrides

        protected override void EventHandlerRegister()
        {
            SceneLoadingService.OnSceneReadyToPlay += OnSceneReadyToPlay;
            SceneLoadingService.OnSceneLoaded += OnSceneLoaded;
            
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
            SceneLoadingService.OnSceneReadyToPlay -= OnSceneReadyToPlay;
            SceneLoadingService.OnSceneLoaded -= OnSceneLoaded;
            
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
            SceneLoadingService.LoadScene(new SceneLoadingData(StartConfig.SceneIndexToLoadOnStart));
            return;
        }
        
        _isGameStarted = true;
        GameManagerHandlerData.GameStarted();
    }

    private void OnSceneLoaded(int sceneIndex, float _)
    {
        // Main menu scene
        if(sceneIndex == StartConfig.MainMenuSceneIndex)
        {
            UIMenuManagerHandlerData.ShowMainMenu();
        }
    }

    private void OnSceneReadyToPlay(int sceneIndex)
    {
        // Game scene
        if(StartConfig.LoadSceneOnStartGame && sceneIndex == StartConfig.SceneIndexToLoadOnStart)
        {
            if(_isGameStarted) return;
            _isGameStarted = true;
            GameManagerHandlerData.GameStarted();
        }
    }
    
    private void StopGame()
    {
        if(!_isGameStarted || SceneLoadingService.IsLoading) return;
        
        Time.timeScale = 1;
        _isGameStarted = _isGamePaused = false;
        GameManagerHandlerData.GameStopped();
        
        if (SceneLoadingService.ActiveScene == StartConfig.MainMenuSceneIndex)
        {
            UIMenuManagerHandlerData.ShowMainMenu();
        }
        else
        {
            SceneLoadingService.LoadScene(new SceneLoadingData(StartConfig.MainMenuSceneIndex));
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