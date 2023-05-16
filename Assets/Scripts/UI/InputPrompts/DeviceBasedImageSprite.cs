using UnityEngine;
using UnityEngine.UI;

namespace UI.InputPrompts
{
    public class DeviceBasedImageSprite : DeviceBasedInputPrompts<Image, Sprite>
    {
        protected override void ApplyPrompt(Sprite prompt) => Graphics.sprite = prompt;
    }
}