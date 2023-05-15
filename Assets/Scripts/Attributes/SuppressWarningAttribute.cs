using System;
using System.Reflection;

namespace Assets.Scripts.Attributes
{
    #region Enum

    [Flags]
    public enum Warnings : uint
    {
        None = 0,

        UnityUpdate = 0x00000001,

        Any = 0xFFFFFFFF,
    }

    #endregion Enum

    public class SuppressWarningAttribute : Attribute
    {
        #region Variables

        Warnings m_warnings;

        #endregion Variables

        #region Functions

        public SuppressWarningAttribute()
        {
            m_warnings = Warnings.Any;
        }

        public SuppressWarningAttribute(Warnings warnings)
        {
            m_warnings = warnings;
        }

        public bool Suppresses(Warnings warning)
        {
            return (m_warnings & warning) != 0;
        }

        public static bool Suppresses(ICustomAttributeProvider provider, Warnings warning)
        {
            object[] attributes = provider.GetCustomAttributes(typeof(SuppressWarningAttribute), true);

            foreach (object attribute in attributes)
            {
                if (((SuppressWarningAttribute)attribute).Suppresses(warning))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Functions
    }
}