using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnPointData",menuName = "SceneData/SpawnPointData",order = 1)]
public class SpawnPointData : ScriptableObject
{
    public string sceneName; //name of the scene (for reference)
    public List<string> spawnPointIDs = new List<string>(); //list of all valid spawn point names
}
