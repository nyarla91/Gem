using UnityEngine;

namespace Extentions
{
    public static class ColorExtentions
    {
        public static Color WithA(this Color color, float newA) => new Color(color.r, color.g, color.b, newA);
        public static Color32 WithA(this Color32 color, byte newA) => new Color(color.r, color.g, color.b, newA);

    }
}
