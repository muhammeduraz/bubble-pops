#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Assets.Scripts.Attributes
{
    public class SerializedValueRequired : PropertyAttribute
    {

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializedValueRequired))]
    public class SerializedValueRequiredDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Color color = GUI.color;
            if (property.objectReferenceValue == null)
            {
                GUI.color = Color.red;
            }
            EditorGUI.PropertyField(position, property, label, true);
            GUI.color = color;
        }
    }
#endif

}