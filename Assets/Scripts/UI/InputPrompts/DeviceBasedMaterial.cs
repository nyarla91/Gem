using UnityEngine;

namespace UI.InputPrompts
{
    public class DeviceBasedMaterial : DeviceBasedInputPrompts<Renderer, Material>
    {
        protected override void ApplyPrompt(Material prompt) => Graphics.material = prompt;
    }
}