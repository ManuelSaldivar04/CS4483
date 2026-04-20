using System.Collections;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    // [SerializeField] private 
    [Header("Normal Scene Transition Activities")]
    [SceneName] public string sceneToLoad; // name of the scene to load
    [SerializeField] private SpawnPointData targetSpawnPointData; // asset for target scene
    [SerializeField] private int selectedSpawnPointIndex; // index into the list
    [SerializeField] private string spawnPointName; // name of the spawn point in the target scene

    [Header("Return Location (for stores / temporary scenes)")]
    [SerializeField] private SpawnPointData returnSpawnPointData; // asset for current scene (where to return)
    [SerializeField] private int returnSpawnPointIndex; // index into that list
    [SerializeField] private string returnSpawnPointName; // auto-filled, the ID to return to

    private LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        if (levelLoader == null)
            Debug.LogError("No LevelLoader found in the scene!", this);
    }

    private void OnValidate()
    {
        // Update spawnPointName for the target scene
        if (targetSpawnPointData != null && targetSpawnPointData.spawnPointIDs != null &&
            selectedSpawnPointIndex >= 0 && selectedSpawnPointIndex < targetSpawnPointData.spawnPointIDs.Count)
        {
            spawnPointName = targetSpawnPointData.spawnPointIDs[selectedSpawnPointIndex];
        }
        else
            spawnPointName = null;

        // Update returnSpawnPointName for the return location
        if (returnSpawnPointData != null && returnSpawnPointData.spawnPointIDs != null &&
            returnSpawnPointIndex >= 0 && returnSpawnPointIndex < returnSpawnPointData.spawnPointIDs.Count)
        {
            returnSpawnPointName = returnSpawnPointData.spawnPointIDs[returnSpawnPointIndex];
        }
        else
            returnSpawnPointName = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelLoader == null || !other.CompareTag("Player"))
            return;

        // If this is a store entrance (return location is defined), remember where to come back
        if (!string.IsNullOrEmpty(returnSpawnPointName))
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            LevelLoader.SetReturnLocation(currentScene, returnSpawnPointName);
        }

        levelLoader.LoadScene(sceneToLoad, spawnPointName);
    }
}