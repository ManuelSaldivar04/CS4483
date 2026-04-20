// LevelLoader.cs (corrected version)

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    // Return location storage (static)
    private static string returnSceneName;
    private static string returnSpawnPointName;  // store the ID string, not the ScriptableObject

    // Static variable for next spawn point (used by LoadScene)
    private static string nextSpawnPointName;

    private void Start()
    {
        if (!string.IsNullOrEmpty(nextSpawnPointName))
        {
            StartCoroutine(PlacePlayerAfterLoad());
        }
    }

    private IEnumerator PlacePlayerAfterLoad()
    {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameObject spawnPoint = GameObject.Find(nextSpawnPointName);
            if (spawnPoint != null)
            {
                Debug.Log($"Player moved to spawn point {nextSpawnPointName}");
                PlayerController controller = player.GetComponent<PlayerController>();
                if (controller != null)
                    controller.SetPosition(spawnPoint.transform.position);
                else
                    player.transform.position = spawnPoint.transform.position;
            }
            else
                Debug.LogWarning($"Spawn Point '{nextSpawnPointName}' not found in scene {SceneManager.GetActiveScene().name}");
        }
        else
            Debug.LogWarning("Player not found in new scene!");

        nextSpawnPointName = null;
    }

    public void LoadScene(string sceneName, string spawnPointName = null)
    {
        nextSpawnPointName = spawnPointName;
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void LoadScene(int sceneIndex, string spawnPointName = null)
    {
        nextSpawnPointName = spawnPointName;
        StartCoroutine(LoadLevelCoroutine(sceneIndex));
    }

    private IEnumerator LoadLevelCoroutine(string sceneName)
    {
        if (transition != null) transition.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadLevelCoroutine(int sceneIndex)
    {
        if (transition != null) transition.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Stores the scene and spawn point (by its ID) to return to later.
    /// </summary>
    public static void SetReturnLocation(string sceneName, string spawnPointID)
    {
        returnSceneName = sceneName;
        returnSpawnPointName = spawnPointID;
    }

    /// <summary>
    /// Loads the previously stored return scene and spawn point.
    /// </summary>
    public void LoadReturnScene()
    {
        if (!string.IsNullOrEmpty(returnSceneName))
        {
            LoadScene(returnSceneName, returnSpawnPointName);
            // Clear after use
            returnSceneName = null;
            returnSpawnPointName = null;
        }
        else
            Debug.LogWarning("No return scene stored. Can't go back.");
    }
}