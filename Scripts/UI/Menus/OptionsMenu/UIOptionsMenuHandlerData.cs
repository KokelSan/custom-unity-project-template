using System;

public static class UIOptionsMenuHandlerData
{
        public static void ShowMenu(Action onMenuHidden) => OnShowMenu?.Invoke(onMenuHidden);
        public static event Action<Action> OnShowMenu;
}