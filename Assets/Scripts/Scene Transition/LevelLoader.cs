using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    //singleton instance
    public static LevelLoader Instance { get; private set; }


    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    //static variable to carry spawn point name across scenes
    private static string nextSpawnPointName;

    private void Awake()
    {
        //singleton enforecment
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);//destroy duplicate
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);//this instance will persist
    }
    private void Start()
    {
        //when this LevelLoader awakens in a new scene, check if we need to place the player
        if (!string.IsNullOrEmpty(nextSpawnPointName))
        {
            StartCoroutine(PlacePlayerAfterLoad());
        }
        
    }

    private IEnumerator PlacePlayerAfterLoad()
    {
        yield return null;

        //find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //find the spawn point by name
            GameObject spawnPoint = GameObject.Find(nextSpawnPointName);
            if (spawnPoint != null)
            {
                //position player at new spawnpoint
                player.transform.position = spawnPoint.transform.position;
                Debug.Log($"Player moved to spawn point {nextSpawnPointName}");
            }
            else
            {
                Debug.LogWarning($"Spawn Point '{nextSpawnPointName}' not found in scene {SceneManager.GetActiveScene().name}");
            }
        }
        else
        {
            Debug.LogWarning("Player not found in new scene!");
        }
        //clear the static variable to prevent accidental reuse
        nextSpawnPointName = null;
    }


    //call this from a UI button to load a scene by name only
    /*
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadLevelCoroutine(sceneIndex));
    }
    */
    public void LoadScene(string sceneName)
    {
        LoadScene(sceneName, null);
    }
    //overload that includes a spawn point name
    public void LoadScene(string sceneName,string spawnPointName)
    {
        //store the spawn pint name statiically for the next scene LevelLoader
        nextSpawnPointName = spawnPointName;
        StartCoroutine(LoadLevelCoroutine(sceneName));

    }

    public void LoadScene(int sceneIndex, string spawnPointName = null)
    {
        nextSpawnPointName = spawnPointName;
        StartCoroutine(LoadLevelCoroutine(sceneIndex));
    }

    //private coroutine that handles the transition animation
    private IEnumerator LoadLevelCoroutine(string sceneName)
    {
        //trigger the trnasition animation
        if(transition != null)
        {
            transition.SetTrigger("start");
        }

        //wait for the animation to finish
        yield return new WaitForSeconds(transitionTime);

        //load the new scene
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadLevelCoroutine(int sceneIndex)
    {
        if(transition != null)
        {
            transition.SetTrigger("start");
        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
}
