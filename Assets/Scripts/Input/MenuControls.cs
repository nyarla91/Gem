namespace Input
{
    public static class MenuControls
    {
        private static MenuActions _actions;

        public static MenuActions Actions => _actions ??= Init();

        private static MenuActions Init()
        {
            MenuActions actions = new MenuActions();
            actions.Enable();
            return actions;
        }
    }
}