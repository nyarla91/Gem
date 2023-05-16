using System;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class SerializedVector2
    {
        public float x;
        public float y;

        public Vector2 Vector
        {
            get => new Vector2(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        public SerializedVector2(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public static explicit operator Vector2(SerializedVector2 vector2) => vector2.Vector;
    }
}