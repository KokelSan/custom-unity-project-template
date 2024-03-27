
public class UIMainMenu : UIButtonMenu
{
    protected override void EventHandlerRegister()
    {
        base.EventHandlerRegister();
        
        UIMenuServiceHandlerData.OnShowMainMenu += PlayShowAnimation;
    }
    
    protected override void EventHandlerUnRegister()
    {
        base.EventHandlerUnRegister();
        
        UIMenuServiceHandlerData.OnShowMainMenu -= PlayShowAnimation;
    }
}
