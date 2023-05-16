using System;

namespace UI
{
    public class WindowActions : MenuAdditionalActions<MenuWindow>
    {
        protected override Func<MenuWindow, bool> TriggerCondition => window => window.IsOpened;
    }
}