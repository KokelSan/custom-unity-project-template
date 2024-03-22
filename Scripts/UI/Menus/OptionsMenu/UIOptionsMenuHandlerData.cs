using System;

public static class UIOptionsMenuHandlerData
{
        public static void ShowMenu(Action onGoBack = null) => OnShowMenu?.Invoke(onGoBack);
        public static event Action<Action> OnShowMenu;
}