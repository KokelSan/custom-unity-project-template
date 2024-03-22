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
        StartButton.onClick.AddListener(OnStartButtonClicked);
        OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }
    
    protected override void EventHandlerUnRegister()
    {
        StartButton.onClick.RemoveAllListeners();
        OptionsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
    }

    private void OnStartButtonClicked()
    {
        Hide();
        GameStateServiceHandlerData.StartGame();
    }

    private void OnOptionsButtonClicked()
    {
        Hide();
        UIOptionsMenuHandlerData.ShowMenu(Show);    
    }

    private void OnExitButtonClicked()
    {
        GameStateServiceHandlerData.ExitGame();
    }
}
