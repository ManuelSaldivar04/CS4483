using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReact : MonoBehaviour
{

    public GameObject NPCText;
    public GameObject Collider;
    // Start is called before the first frame update
    void Start()
    {
        NPCText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entered the trigger");
        if (other.gameObject.tag == "Player")
        {
            NPCText.SetActive(true);
            Collider.SetActive(false);
        }
    }

    
}
