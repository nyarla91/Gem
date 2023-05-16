using UnityEngine;

namespace Extentions
{
    public static class MathExtentions
    {
        public static int Sign(this float n)
        {
            if (n == 0)
                return 0;
            return n > 0 ? 1 : -1;
        }

        public static float WithSign(this float n, int sign) => Mathf.Abs(n) * Sign(sign);

        public static bool InBounds(this float number, float max, float min) => (number >= min && number <= max);

        public static bool InBounds(this float number, float bound) =>  InBounds(number, bound, -bound);

        public static float Snap(this float value, float step) => step == 0 ? value : Mathf.Round(value / step) * step;
        
        public static float TimeSin(float min, float max, float timeScale = 1, float timeOffset = 0)
        {
            float sin = Mathf.Sin(Time.time * timeScale + timeOffset);
            return Mathf.Lerp(min, max, (sin + 1) * 0.5f);
        }

        public static float Average(float[] numbers)
        {
            if (numbers.Length == 0)
                return 0;
            if (numbers.Length == 1)
                return numbers[0];
            float total = 0;
            foreach (var number in numbers)
            {
                total += number;
            }
            return total / numbers.Length;
        }

        public static void SetMax(ref float a, float b) => a = Mathf.Max(a, b);
        public static void SetMax(ref int a, int b) => a = Mathf.Max(a, b);
        public static void SetMin(ref float a, float b) => a = Mathf.Min(a, b);
        public static void SetMin(ref int a, int b) => a = Mathf.Min(a, b);

        public static bool IsEven(this int value) => value % 2 == 0;
        
        public static float ClampAngle(this float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);

            if (min < max)
            {
                if (angle.InBounds(max, min))
                    return angle;

                return Mathf.DeltaAngle(angle, min) < Mathf.DeltaAngle(angle, max) ? min : max;
            }
            if (angle > min || angle < max)
                return angle;

            return Mathf.Abs(angle - min) < Mathf.Abs(angle - max) ? min : max;
        }

        public static float EvaluateLine(float a, float b, float t)
        {
            t = Mathf.Clamp(t, 0, 1);
            return a + (b - a) * t;
        }

        public static bool ApproximatelyEqual(this float a, float b, float tolerance)
        {
            tolerance = Mathf.Max(tolerance, 0);
            return Mathf.Abs(a - b) < tolerance;
        }
    }
}