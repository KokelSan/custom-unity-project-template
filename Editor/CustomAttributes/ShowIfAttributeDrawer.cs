using UnityEditor;
using UnityEngine;

namespace CustomAttributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawer : PropertyDrawer
    {
        private ShowIfAttribute _showIf;
        private SerializedProperty _comparedProperty;
        private float _propertyHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _showIf = attribute as ShowIfAttribute;
            if (_showIf == null) return;

            string path = property.propertyPath.Contains(".")
                ? System.IO.Path.ChangeExtension(property.propertyPath, _showIf.ComparedPropertyName)
                : _showIf.ComparedPropertyName;
            _comparedProperty = property.serializedObject.FindProperty(path);

            if (_comparedProperty == null)
            {
                Debug.LogWarning($"Error with ShowIf Attribute, expectedValue not found");
                return;
            }

            bool show = false;
            switch (_comparedProperty.type)
            {
                case "bool":
                    show = _comparedProperty.boolValue.Equals(_showIf.ExpectedValue);
                    break;

                case "Enum":
                    show = _comparedProperty.enumValueIndex.Equals((int)_showIf.ExpectedValue);
                    break;

                default:
                    Debug.LogWarning(
                        $"ShowIfAttribute Error: {_comparedProperty.type} is not a supported type, only bool and Enum are ");
                    break;
            }

            if (show)
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