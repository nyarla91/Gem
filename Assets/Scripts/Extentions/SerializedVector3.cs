using System;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class SerializedVector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3 Vector
        {
            get => new Vector3(x, y, z);
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public SerializedVector3(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static explicit operator Vector3(SerializedVector3 vector3) => vector3.Vector;
    }
}