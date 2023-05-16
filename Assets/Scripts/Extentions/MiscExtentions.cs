using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Extentions
{
    public static class MiscExtentions
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T swapper = a;
            a = b;
            b = swapper;
        }

        public static void SelfDestruct(this Object gameObject)
        {
            Object.Destroy(gameObject);
        }

        public static void Stop(this Coroutine coroutine, MonoBehaviour container, ref Coroutine fieldToClear)
        {
            coroutine.Stop(container);
            fieldToClear = null;
        }

        public static void Stop(this Coroutine coroutine, MonoBehaviour container)
        {
            container.StopCoroutine(coroutine);
        }

        public static void SetAlpha(this Image image, float alpha)
        {
            Color originColor = image.color;
            image.color = new Color(originColor.r, originColor.g, originColor.b, alpha);
        }

        public static bool NullOrAssign<T>(this T obj, out T target)
        {
            target = obj;
            return target != null;
        }

        public static Vector3 DirectionTo(this Transform transform, Vector3 target) => (target - transform.position).normalized;
        public static Vector3 DirectionTo(this Transform transform, Transform target) => transform.DirectionTo(target.position);
        public static Vector2 DirectionTo2D(this Transform transform, Vector2 target) => (target - (Vector2) transform.position).normalized;
        public static Vector2 DirectionTo2D(this Transform transform, Transform target) => transform.DirectionTo2D(target.position);

        public static async Task WaitForCondition<T>(this T original, Func<T, bool> condition, int checkPeriod)
        {
            while (!condition.Invoke(original))
            {
                await Task.Delay(checkPeriod);
            }
        }

        public static T With<T>(this T obj, Action<T> action)
        {
            action.Invoke(obj);
            return obj;
        }
    }
}