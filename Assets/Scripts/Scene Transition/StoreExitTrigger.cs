using UnityEngine;

public class StoreExitTrigger : MonoBehaviour
{
    private LevelLoader levelLoader;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && levelLoader != null)
        {
            levelLoader.LoadReturnScene();
        }
    }
}