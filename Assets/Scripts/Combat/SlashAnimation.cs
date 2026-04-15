using System.Collections;
using UnityEngine;

public class SlashAnimation : MonoBehaviour
{
    public Animator animator;

    public void PlaySlash(System.Action onComplete)
    {
   
        gameObject.SetActive(true);
        StartCoroutine(Play(onComplete));
        
    }

    IEnumerator Play(System.Action onComplete)
    {
        GameManager.Instance.resultText.gameObject.SetActive(true);
        animator.enabled = true;
        animator.Play("Slash");

        yield return new WaitForSeconds(0.35f);

        animator.enabled = false;
        gameObject.SetActive(false);


        onComplete?.Invoke();
    }
}