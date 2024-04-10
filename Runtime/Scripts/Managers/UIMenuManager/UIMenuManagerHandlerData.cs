using System;

public static class UIMenuManagerHandlerData
{
    public static void ShowMainMenu(bool showInstant = false, Action onMenuHidden = null) => OnShowMainMenu?.Invoke(showInstant, onMenuHidden);
    public static event Action<bool, Action> OnShowMainMenu;
    
    public static void ShowOptionsMenu(bool showInstant = false, Action onGoBack = null) => OnShowOptionsMenu?.Invoke(showInstant, onGoBack);
    public static event Action<bool, Action> OnShowOptionsMenu;
}