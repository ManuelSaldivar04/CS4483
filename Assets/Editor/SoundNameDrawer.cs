// Editor/SoundNameDrawer.cs
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SoundNameAttribute))]
public class SoundNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use [SoundName] with string.");
            return;
        }

        SoundNameAttribute attr = (SoundNameAttribute)attribute;
        var targetObject = property.serializedObject.targetObject;

        // Find the SoundEffectLibrary field
        var libraryField = targetObject.GetType().GetField(attr.libraryFieldName,
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        SoundEffectLibrary library = null;
        if (libraryField != null)
        {
            library = libraryField.GetValue(targetObject) as SoundEffectLibrary;
        }
        else
        {
            // Fallback: search for component on same GameObject
            if (targetObject is MonoBehaviour mb)
            {
                library = mb.GetComponent<SoundEffectLibrary>();
            }
        }

        if (library == null)
        {
            EditorGUI.LabelField(position, label.text, "No SoundEffectLibrary found.");
            return;
        }

        // Get sound names using the new public method
        string[] soundNames = library.GetAllSoundGroupNames();
        if (soundNames.Length == 0)
        {
            EditorGUI.LabelField(position, label.text, "No sound groups defined.");
            return;
        }

        int currentIndex = System.Array.IndexOf(soundNames, property.stringValue);
        if (currentIndex < 0) currentIndex = 0;

        EditorGUI.BeginProperty(position, label, property);
        int newIndex = EditorGUI.Popup(position, label.text, currentIndex, soundNames);
        if (newIndex != currentIndex)
        {
            property.stringValue = soundNames[newIndex];
        }
        EditorGUI.EndProperty();
    }
}