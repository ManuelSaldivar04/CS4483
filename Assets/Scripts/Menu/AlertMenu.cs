
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AlertMenu : MonoBehaviour
{
    public GameObject alertText;
    private TextMeshProUGUI tmp;
    private Queue<string> alertQueue = new Queue<string>();
    private bool isShowing;
    public float displayTime = 3f;
    public float fadeTime = 1f;

    void Awake()
    {
        tmp = alertText.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void ShowAlert(string text)
    {
        alertQueue.Enqueue(text);

        if (!isShowing)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (alertQueue.Count > 0)
        {
            string text = alertQueue.Dequeue();
            yield return StartCoroutine(ShowSingleAlert(text));
        }

        isShowing = false;
    }  

    IEnumerator ShowSingleAlert(string text)
    {

        tmp.text = text;
        SetAlpha(1f);
        alertText.SetActive(true);
        
        yield return new WaitForSecondsRealtime(displayTime);   

        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
        alertText.SetActive(false);
    }

    IEnumerator ShowAndFade()
    {
        // Ensure fully visible
        SetAlpha(1f);
        alertText.SetActive(true);

        // Wait for display time
        yield return new WaitForSecondsRealtime(displayTime);

        // Fade out
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            SetAlpha(alpha);
            yield return null;
        }

        // Ensure fully invisible at end
        SetAlpha(0f);
        alertText.SetActive(false);
    }

    void SetAlpha(float alpha)
    {
        Color c = tmp.color;
        c.a = alpha;
        tmp.color = c;
    }
}
