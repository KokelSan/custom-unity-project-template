
public class UIMainMenu : UIButtonMenu
{
    protected override void EventHandlerRegister()
    {
        base.EventHandlerRegister();
        
        UIMenuManagerHandlerData.OnShowMainMenu += PlayShowAnimation;
    }
    
    protected override void EventHandlerUnRegister()
    {
        base.EventHandlerUnRegister();
        
        UIMenuManagerHandlerData.OnShowMainMenu -= PlayShowAnimation;
    }
}
