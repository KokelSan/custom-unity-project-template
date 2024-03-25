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

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _propertyHeight;
        }

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
            switch (_comparedProperty.type)
            {
                case "bool":
                    hide = _comparedProperty.boolValue.Equals(_hideIf.ExpectedValue);
                    break;

                case "Enum":
                    hide = _comparedProperty.enumValueIndex.Equals((int)_hideIf.ExpectedValue);
                    break;

                default:
                    Debug.LogWarning($"HideIfAttribute Error: {_comparedProperty.type} is not a supported type, only bool and Enum are ");
                    break;
            }

            if (!hide)
            {
                _propertyHeight = base.GetPropertyHeight(property, label);
                EditorGUI.PropertyField(position, property);
            }
            else
            {
                _propertyHeight = 0f;
            }
        }
    }
}