#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace KokelSan.CustomAttributes
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawer : PropertyDrawer
    {
        private HideIfAttribute _hideIf;
        private SerializedProperty _comparedProperty;
        private float _propertyHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _hideIf = attribute as HideIfAttribute;
            if (_hideIf == null) return;

            string path = property.propertyPath.Contains(".")
                ? System.IO.Path.ChangeExtension(property.propertyPath, _hideIf.ComparedPropertyName)
                : _hideIf.ComparedPropertyName;
            _comparedProperty = property.serializedObject.FindProperty(path);

            if (_comparedProperty == null)
            {
                Debug.LogWarning($"Error with HideIf Attribute, expectedValue not found");
                return;
            }

            bool hide = false;
            switch (_comparedProperty.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    hide = _comparedProperty.boolValue.Equals(_hideIf.ExpectedValue);
                    break;

                case SerializedPropertyType.Enum:
                    hide = _comparedProperty.enumValueIndex.Equals((int)_hideIf.ExpectedValue);
                    break;

                default:
                    Debug.LogWarning($"HideIfAttribute Error: {_comparedProperty.type} is not a supported type, only bool and Enum are ");
                    break;
            }

            if (hide)
            {
                _propertyHeight = -EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                _propertyHeight = EditorGUIUtility.standardVerticalSpacing;
                
                int originalIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = property.depth;
                EditorGUILayout.PropertyField(property);
                EditorGUI.indentLevel = originalIndentLevel;
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _propertyHeight;
        }
    }
}

#endif