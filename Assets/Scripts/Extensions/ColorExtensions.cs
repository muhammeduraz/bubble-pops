using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithR(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }

        public static Color WithG(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }

        public static Color WithB(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }

        public static Color WithA(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }

        public static float Distance(this Color color, Color other)
        {
            return Mathf.Max(
<<<<<<< Updated upstream
        Mathf.Abs(other.r - color.r),
        Mathf.Abs(other.g - color.g),
        Mathf.Abs(other.b - color.b),
        Mathf.Abs(other.a - color.a)
          );
=======
                Mathf.Abs(other.r - color.r),
                Mathf.Abs(other.g - color.g),
                Mathf.Abs(other.b - color.b),
                Mathf.Abs(other.a - color.a)
                );
>>>>>>> Stashed changes
        }
    }
}