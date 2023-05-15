using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class VectorExtensions
    {
<<<<<<< Updated upstream
        public static Vector2 WithX(this Vector2 vector, float x)
=======
        #region Functions

        public static Vector2 WithX(this Vector2 vector, float x = 0f)
>>>>>>> Stashed changes
        {
            return new Vector2(x, vector.y);
        }

<<<<<<< Updated upstream
        public static Vector2 WithY(this Vector2 vector, float y)
=======
        public static Vector2 WithY(this Vector2 vector, float y = 0f)
>>>>>>> Stashed changes
        {
            return new Vector2(vector.x, y);
        }

<<<<<<< Updated upstream
        public static Vector3 WithX(this Vector3 vector, float x)
=======
        public static Vector3 WithX(this Vector3 vector, float x = 0f)
>>>>>>> Stashed changes
        {
            return new Vector3(x, vector.y, vector.z);
        }

<<<<<<< Updated upstream
        public static Vector3 WithY(this Vector3 vector, float y)
=======
        public static Vector3 WithY(this Vector3 vector, float y = 0f)
>>>>>>> Stashed changes
        {
            return new Vector3(vector.x, y, vector.z);
        }

<<<<<<< Updated upstream
        public static Vector3 WithZ(this Vector3 vector, float z)
=======
        public static Vector3 WithZ(this Vector3 vector, float z = 0f)
>>>>>>> Stashed changes
        {
            return new Vector3(vector.x, vector.y, z);
        }

<<<<<<< Updated upstream
        public static Vector3 WithXY(this Vector3 vector, float x, float y)
=======
        public static Vector3 WithXY(this Vector3 vector, float x = 0f, float y = 0f)
>>>>>>> Stashed changes
        {
            return new Vector3(x, y, vector.z);
        }

<<<<<<< Updated upstream
        public static Vector3 WithXZ(this Vector3 vector, float x, float z)
=======
        public static Vector3 WithXZ(this Vector3 vector, float x = 0f, float z = 0f)
>>>>>>> Stashed changes
        {
            return new Vector3(x, vector.y, z);
        }

<<<<<<< Updated upstream
        public static Vector3 WithYZ(this Vector3 vector, float y, float z)
        {
            return new Vector3(vector.x, y, z);
        }
=======
        public static Vector3 WithYZ(this Vector3 vector, float y = 0f, float z = 0f)
        {
            return new Vector3(vector.x, y, z);
        }

        #endregion Functions
>>>>>>> Stashed changes
    }
}