using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCombat : MonoBehaviour
{
    public GameObject[] slides;
    int x;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < slides.Length; i++)
            slides[i].SetActive(false);

        x = 0;
        slides[x].SetActive(true);
    }

    public void nextSlide()
    {
        x++;
        if (x == 14)
            SceneManager.LoadScene("Tutorial");
        else
        {
            x--;
            slides[x].SetActive(false);
            x++;
            slides[x].SetActive(true);
        }
    }
    
}
