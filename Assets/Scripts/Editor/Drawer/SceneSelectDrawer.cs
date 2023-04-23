using System;
using Attribute;
using UnityEditor;
using UnityEngine;
namespace Editor
{
    [CustomPropertyDrawer(typeof(SceneSelectAttribute))]
    public class SceneSelectDrawer : PropertyDrawer
    {
        private const string WarningInfo = "Use SceneSelect with string or int ";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var nameList = (attribute as SceneSelectAttribute)?.nameList;
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                {
                    if (nameList == null) return;
                    var selectedIndex = Mathf.Max(0, Array.IndexOf(nameList, property.stringValue));
                    var index = EditorGUI.Popup(position, property.displayName, selectedIndex, nameList);
                    property.stringValue = nameList[index];
                    break;
                }
                case SerializedPropertyType.Integer:
                    if (nameList == null) return;
                    property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, nameList);
                    break;
                default:
                    EditorGUI.LabelField(position, property.displayName, WarningInfo, EditorStyles.label);
                    break;
            }
        }
    }
}
