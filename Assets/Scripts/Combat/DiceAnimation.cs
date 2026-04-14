using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceAnimation : MonoBehaviour
{
    public Animator animator;

    
    public void RollDice(System.Action onComplete)
    {
        StartCoroutine(PlayRoll(onComplete));
    }

    IEnumerator PlayRoll(System.Action onComplete)
    {
        // start spinning
        animator.enabled = true;
        animator.Play("DiceRoll");

        // spin for 1 second
        yield return new WaitForSeconds(2f);

        // stop animation
        animator.enabled = false;

        onComplete?.Invoke();
    }
}