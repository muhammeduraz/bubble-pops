using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using UnityEngine.Assertions;
using Assets.Scripts.Attributes;
using System.Collections.Generic;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Assets.Scripts.Mono
{
    public static class MonoBehaviourExt
    {
        #region Functions

        [Conditional("UNITY_EDITOR")]
        public static void AssertAllRequiredValuesSerialized(this MonoBehaviour mb)
        {
            foreach (FieldInfo field in GetAllFields(mb.GetType()))
            {
                var att = field.GetCustomAttributes(typeof(SerializedValueRequired), true).FirstOrDefault();
                if (att != null)
                {
                    Assert.AreNotEqual(null, field.GetValue(mb), $"{field} in {mb} hasn't been serialized!");
                    Assert.AreNotEqual("null", field.GetValue(mb).ToString(), $"{field} in {mb} hasn't been serialized!");
                }
            }
        }

        private static IEnumerable<FieldInfo> GetAllFields(Type t)
        {
            if (t == null)
                return Enumerable.Empty<FieldInfo>();

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Static | BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;
            return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
        }

        #endregion Functions
    }
}