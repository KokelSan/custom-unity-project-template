using System;

public static class UIMainMenuHandlerData
{
    public static void ShowMenu(Action onMenuHidden = null) => OnShowMenu?.Invoke(onMenuHidden);
    public static event Action<Action> OnShowMenu;
}