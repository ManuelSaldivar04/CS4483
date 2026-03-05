using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; //name of the scene to load
    [SerializeField] private SpawnPointData targetSpawnPointData; //asset for target scene
    [SerializeField] private int selectedSpawnPointIndex; //index into the list
    [SerializeField] private string spawnPointName; //name of the spawn point in the target scene
    [SerializeField] private LevelLoader levelLoader; //need to reference the LevelLoader script

    private void OnValidate()
    {
        //update spawnPointName when the index or data changes
        if(targetSpawnPointData != null && targetSpawnPointData.spawnPointIDs != null && selectedSpawnPointIndex >= 0 && selectedSpawnPointIndex < targetSpawnPointData.spawnPointIDs.Count)
        {
            spawnPointName = targetSpawnPointData.spawnPointIDs[selectedSpawnPointIndex];
        }
        else
        {
            spawnPointName = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(levelLoader != null)
        {
            if (other.CompareTag("Player"))
            {
                levelLoader.LoadScene(sceneToLoad,spawnPointName); //load the appropriate scene when collide with Player tag
                
            }
        }
    }
}
