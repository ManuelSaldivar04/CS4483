using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("NPC Attributes")]
    public string npcName;
    public Sprite npcPortrait;
    [TextArea(3,5)]
    public string[] dialogueLines;
    public bool[] autoProgressLines;

    [Header("NPC Speaking Attributes")]
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public float autoProgressDelay = 1.5f;

}
