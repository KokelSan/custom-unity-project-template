using System;

public static class UIMenuServiceHandlerData
{
    public static void ShowMainMenu(Action onMenuHidden = null) => OnShowMainMenu?.Invoke(onMenuHidden);
    public static event Action<Action> OnShowMainMenu;
    
    public static void ShowOptionsMenu(Action onGoBack = null) => OnShowOptionsMenu?.Invoke(onGoBack);
    public static event Action<Action> OnShowOptionsMenu;
}