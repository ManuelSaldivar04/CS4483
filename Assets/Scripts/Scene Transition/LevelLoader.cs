using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    //call this from a UI button to load a scene by name
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void LoadScene(int sceneIndex)
    {
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
