using System;
using UnityEngine;
using UnityEngine.UI;


public class UIMainMenu : UIAnimatedElement
{
    [Header("Main Menu Elements")]
    public Button StartButton;
    public Button OptionsButton;
    public Button ExitButton;

    protected override void EventHandlerRegister()
    {
        UIMainMenuHandlerData.OnShowMenu += ShowMenu;
        
        StartButton.onClick.AddListener(OnStartButtonClicked);
        OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }
    
    protected override void EventHandlerUnRegister()
    {
        UIMainMenuHandlerData.OnShowMenu -= ShowMenu;
        
        StartButton.onClick.RemoveAllListeners();
        OptionsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
    }

    private void ShowMenu(Action onMenuHidden)
    {
        PlayShowAnimation(onMenuHidden);
    }

    private void OnStartButtonClicked()
    {
        PlayHideAnimation();
        GameManagerHandlerData.StartGame();
    }

    private void OnOptionsButtonClicked()
    {
        PlayHideAnimation();
        UIOptionsMenuHandlerData.ShowMenu(() => PlayShowAnimation());    
    }

    private void OnExitButtonClicked()
    {
        GameManagerHandlerData.ExitGame();
    }
}
