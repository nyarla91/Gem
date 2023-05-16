using TMPro;

namespace UI.InputPrompts
{
    public class DeviceBasedSpriteAsset : DeviceBasedInputPrompts<TMP_Text, TMP_SpriteAsset>
    {
        protected override void ApplyPrompt(TMP_SpriteAsset prompt) => Graphics.spriteAsset = prompt;
    }
}