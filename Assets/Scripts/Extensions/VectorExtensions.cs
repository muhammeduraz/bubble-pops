using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector3 WithXY(this Vector3 vector, float x, float y)
        {
            return new Vector3(x, y, vector.z);
        }

        public static Vector3 WithXZ(this Vector3 vector, float x, float z)
        {
            return new Vector3(x, vector.y, z);
        }

        public static Vector3 WithYZ(this Vector3 vector, float y, float z)
        {
            return new Vector3(vector.x, y, z);
        }
    }
}