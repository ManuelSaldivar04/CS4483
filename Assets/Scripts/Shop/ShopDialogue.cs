using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Dialogue", menuName = "Shop/Shop Dialogue")]
public class ShopDialogue : ScriptableObject
{
    [Header("Shop Dialogue Lines")]
    public List<string> basicLines = new List<string>();
    public List<string> buyLines = new List<string>();

    [Header("Shop Dialogue Attributes")]
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
}
