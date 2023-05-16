using System;

namespace UI.UIElements
{
    public class UIElementActions : MenuAdditionalActions<UIElement>
    {
        protected override Func<UIElement, bool> TriggerCondition => element => element.IsSelected;
    }
}