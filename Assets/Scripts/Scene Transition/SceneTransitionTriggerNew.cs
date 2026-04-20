using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTriggerNew : MonoBehaviour
{
    [Header("New scene transition")]
    [SerializeField] private string sceneToLoad; //name of the scene to load
    [SerializeField] private SpawnPointData targetSpawnPointData; //asset for target scene
    [SerializeField] private int selectedSpawnPointIndex; //index into the list
    [SerializeField] private string spawnPointName; //name of the spawn point in the target scene
    public string hello;
}
