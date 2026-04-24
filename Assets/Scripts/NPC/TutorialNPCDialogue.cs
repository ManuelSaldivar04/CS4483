using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial NPCDialogue", menuName = "Tutorial NPC Dialogue")]
public class TutorialNPCDialogue : ScriptableObject
{
    [Header("NPC Attributes")]
    public string npcName;
    public Sprite npcPortrait;
    [TextArea(3,5)]
    public string[] dialogueLines1;
    public string[] dialogueLines2;
    public string[] dialogueLines3;
    public string[] dialogueLines4;

    [Header("Combat Settings")]
    public bool CombatEnemy;
    public float combatTransitionDelay = 2f; //seconds to wait after dialogue ends
    [SceneName] public string combatSceneName = "Combat";
    public EnemyData enemyData;

    [Header("NPC Speaking Attributes")]
    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public float autoProgressDelay = 1.5f;

}
