using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : UIAnimatedElement
{
    [Header("Pause Menu Elements")]
    public Button ResumeButton;
    public Button OptionsButton;
    public Button MainMenuButton;
    
    protected override void EventHandlerRegister()
    {
        GameManagerHandlerData.OnGamePaused += OnGamePaused;
        GameManagerHandlerData.OnGameResumed += OnGameResumed;
        
        ResumeButton.onClick.AddListener(OnResumeButtonClicked);
        OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
        MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    protected override void EventHandlerUnRegister()
    {
        GameManagerHandlerData.OnGamePaused -= OnGamePaused;
        GameManagerHandlerData.OnGameResumed -= OnGameResumed;
        
        ResumeButton.onClick.RemoveAllListeners();
        OptionsButton.onClick.RemoveAllListeners();
        MainMenuButton.onClick.RemoveAllListeners();
    }
    
    private void OnGamePaused()
    {
        PlayShowAnimation();
    }
    
    private void OnGameResumed()
    {
        PlayHideAnimation();
    }

    private void OnResumeButtonClicked()
    {
        GameManagerHandlerData.ResumeGame();
    }

    private void OnOptionsButtonClicked()
    {
        PlayHideAnimation();
        UIOptionsMenuHandlerData.ShowMenu(() => PlayShowAnimation());    
    }

    private void OnMainMenuButtonClicked()
    {
        PlayHideAnimation();
        GameManagerHandlerData.StopGame();
    }
}