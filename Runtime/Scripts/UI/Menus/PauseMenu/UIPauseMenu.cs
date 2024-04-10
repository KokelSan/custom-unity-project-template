
public class UIPauseMenu : UIButtonMenu
{
    protected override void EventHandlerRegister()
    {
        base.EventHandlerRegister();
        
        GameManagerHandlerData.OnGamePaused += OnGamePaused;
        GameManagerHandlerData.OnGameResumed += OnGameResumed;
    }

    protected override void EventHandlerUnRegister()
    {
        base.EventHandlerUnRegister();
        
        GameManagerHandlerData.OnGamePaused -= OnGamePaused;
        GameManagerHandlerData.OnGameResumed -= OnGameResumed;
    }
    
    private void OnGamePaused()
    {
        Show();
    }
    
    private void OnGameResumed()
    {
        Hide();
    }
}