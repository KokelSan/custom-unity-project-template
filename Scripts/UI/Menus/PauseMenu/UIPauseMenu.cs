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
        GameStateServiceHandlerData.OnGamePaused += OnGamePaused;
        GameStateServiceHandlerData.OnGameResumed += OnGameResumed;
        
        ResumeButton.onClick.AddListener(OnResumeButtonClicked);
        OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
        MainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    protected override void EventHandlerUnRegister()
    {
        GameStateServiceHandlerData.OnGamePaused -= OnGamePaused;
        GameStateServiceHandlerData.OnGameResumed -= OnGameResumed;
        
        ResumeButton.onClick.RemoveAllListeners();
        OptionsButton.onClick.RemoveAllListeners();
        MainMenuButton.onClick.RemoveAllListeners();
    }
    
    private void OnGamePaused()
    {
        Show();
    }
    
    private void OnGameResumed()
    {
        Hide();
    }
    

    private void OnResumeButtonClicked()
    {
        GameStateServiceHandlerData.ResumeGame();
    }

    private void OnOptionsButtonClicked()
    {
        Hide();
        UIOptionsMenuHandlerData.ShowMenu(Show);    
    }

    private void OnMainMenuButtonClicked()
    {
        Hide();
        GameStateServiceHandlerData.StopGame();
    }
}