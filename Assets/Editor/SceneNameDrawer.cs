// Editor/SceneNameDrawer.cs
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use [SceneName] with string.");
            return;
        }

        // Get all valid scene names from Build Settings
        var scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
            .ToArray();

        if (scenes.Length == 0)
        {
            EditorGUI.LabelField(position, label.text, "No scenes in Build Settings");
            return;
        }

        // Find current selected index
        int currentIndex = System.Array.IndexOf(scenes, property.stringValue);
        if (currentIndex < 0) currentIndex = 0; // default to first scene if not found

        // Draw dropdown
        EditorGUI.BeginProperty(position, label, property);
        int newIndex = EditorGUI.Popup(position, label.text, currentIndex, scenes);
        if (newIndex != currentIndex)
        {
            property.stringValue = scenes[newIndex];
        }
        EditorGUI.EndProperty();
    }
}