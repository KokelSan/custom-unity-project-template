using System;

public static class OptionsMenuHandlerData
{
        public static void ShowMenu(Action onMenuHidden) => OnShowMenu?.Invoke(onMenuHidden);
        public static event Action<Action> OnShowMenu;
        
        public static void MenuHidden() => OnMenuHidden?.Invoke();
        public static event Action OnMenuHidden;
}