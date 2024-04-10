
public class UIMainMenu : UIButtonMenu
{
    protected override void EventHandlerRegister()
    {
        base.EventHandlerRegister();
        
        UIMenuManagerHandlerData.OnShowMainMenu += Show;
    }
    
    protected override void EventHandlerUnRegister()
    {
        base.EventHandlerUnRegister();
        
        UIMenuManagerHandlerData.OnShowMainMenu -= Show;
    }
}
