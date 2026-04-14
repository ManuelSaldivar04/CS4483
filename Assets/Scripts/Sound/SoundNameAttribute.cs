// SoundNameAttribute.cs
using UnityEngine;

public class SoundNameAttribute : PropertyAttribute
{
    // Optionally, you can specify a field name if your library is stored in a different variable
    public string libraryFieldName = "soundEffectLibrary";
    public SoundNameAttribute(string libraryFieldName = "soundEffectLibrary")
    {
        this.libraryFieldName = libraryFieldName;
    }
}