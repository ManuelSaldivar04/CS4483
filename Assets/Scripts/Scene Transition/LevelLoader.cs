
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

    private static Vector3? returnPosition; //spawn player in same position after combat
    private static Vector3? nextSpawnPosition;

    // Static variable for next spawn point (used by LoadScene)
    private static string nextSpawnPointName;

    private void Start()
    {
        if (!string.IsNullOrEmpty(nextSpawnPointName) || nextSpawnPosition.HasValue)
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
            // PRIORITY: use exact position if set
            if (nextSpawnPosition.HasValue)
            {
                PlayerController controller = player.GetComponent<PlayerController>();
                if (controller != null)
                    controller.SetPosition(nextSpawnPosition.Value);
                else
                    player.transform.position = nextSpawnPosition.Value;
                Debug.Log($"Player placed at custom position {player.transform.position}");
                nextSpawnPosition = null;
            }
            else if (!string.IsNullOrEmpty(nextSpawnPointName))
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
            nextSpawnPointName = null;
        }
        else
        {
            Debug.LogWarning("Player not found in new scene!");
            nextSpawnPointName = null;
            nextSpawnPosition = null;
        }
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

    public static void SetReturnLocation(string sceneName, Vector3 position)
    {
        returnSceneName = sceneName;
        returnPosition = position;
        returnSpawnPointName = null;
    }
    
    public static void SetReturnLocation(string sceneName, string spawnPointID)
    {
        returnSceneName = sceneName;
        returnSpawnPointName = spawnPointID;
    }

    
    public void LoadReturnScene()
    {
        if (!string.IsNullOrEmpty(returnSceneName))
        {
            if (returnPosition.HasValue)
            {
                //store the exact position to be used after scene load
                nextSpawnPosition = returnPosition.Value;
                //load the scene
                LoadScene(returnSceneName, null);
            }
            else if (!string.IsNullOrEmpty(returnSpawnPointName))
            {
                LoadScene(returnSceneName, returnSpawnPointName);
            }
            else
            {
                Debug.LogWarning("No return location (neither position nor spawn point) stored");
                return;
            }
                //LoadScene(returnSceneName, returnSpawnPointName);
            // Clear after use
            returnSceneName = null;
            returnSpawnPointName = null;
            returnPosition = null;
        }
        else
            Debug.LogWarning("No return scene stored. Can't go back.");
    }
}