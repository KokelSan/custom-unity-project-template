using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UIAnimatedElement
{
    [Header("Main Menu Elements")]
    public Button StartButton;
    public Button ParametersButton;
    public Button ExitButton;

    protected override void EventHandlerRegister()
    {
        StartButton.onClick.AddListener(OnStartButtonClicked);
        ParametersButton.onClick.AddListener(OnParametersButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }
    
    protected override void EventHandlerUnRegister()
    {
        StartButton.onClick.RemoveAllListeners();
        ParametersButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
    }

    private void OnStartButtonClicked()
    {
        GameManagerManagerHandlerData.StartGame();
    }

    private void OnParametersButtonClicked()
    {
        Debug.Log("Parameters menu's button clicked");
    }

    private void OnExitButtonClicked()
    {
        GameManagerManagerHandlerData.ExitGame();
    }
}
