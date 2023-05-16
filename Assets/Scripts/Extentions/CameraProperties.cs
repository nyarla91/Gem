using UnityEngine;

namespace Extentions
{
    [RequireComponent(typeof(Camera))]
    public class CameraProperties : Transformable
    {
        private static CameraProperties _instance;
        public static CameraProperties Instance => _instance;
        
        private Camera _main;
        public Camera Main => _main ??= Camera.main;

        public float YRotation => _instance.Transform.rotation.eulerAngles.y;
        

        public static Vector3 PercentToScreenPoint(Vector2 percent)
        {
            percent += Vector2.one;
            percent /= 2;
            return new Vector2(Screen.width, Screen.height) * percent;
        }

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            _main = null;
        }
    }
}
