using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSlashAnimation : MonoBehaviour
{
    public Animator animator;

    public void PlaySlash(System.Action onComplete)
    {

        gameObject.SetActive(true);
        StartCoroutine(Play(onComplete));

    }

    IEnumerator Play(System.Action onComplete)
    {
        animator.enabled = true;
        animator.Play("MirrorSlash");

        yield return new WaitForSeconds(0.35f);

        animator.enabled = false;
        gameObject.SetActive(false);

        onComplete?.Invoke();
    }
}
