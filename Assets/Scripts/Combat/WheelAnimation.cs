using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WheelAnimation : MonoBehaviour
{
    public Animator animator;
    public Image wheelImage;
    public Sprite idleSprite;

    public float singleSpinDuration = 1.5f;
    public int spinCycles = 1;

    private Coroutine currentSpin;

    private void Awake()
    {
        ResetToIdle();
    }

    public void ResetToIdle()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (wheelImage == null)
            wheelImage = GetComponent<Image>();

        if (animator != null)
            animator.enabled = false;

        if (wheelImage != null && idleSprite != null)
            wheelImage.sprite = idleSprite;
    }

    public void SpinWheel(System.Action onComplete)
    {
        Debug.Log("[WheelAnimation] SpinWheel() was called");

        if (currentSpin != null)
            StopCoroutine(currentSpin);

        currentSpin = StartCoroutine(PlaySpin(onComplete));
    }

    private IEnumerator PlaySpin(System.Action onComplete)
    {
        Debug.Log("[WheelAnimation] PlaySpin() started");

        if (animator == null)
            animator = GetComponent<Animator>();

        if (wheelImage == null)
            wheelImage = GetComponent<Image>();

        if (animator == null)
        {
            Debug.LogError("[WheelAnimation] Animator is NULL");
            ResetToIdle();
            onComplete?.Invoke();
            yield break;
        }

        animator.enabled = true;

        for (int i = 0; i < spinCycles; i++)
        {
            Debug.Log("[WheelAnimation] Playing spin cycle " + (i + 1));
            animator.Play("WheelSpin", 0, 0f);
            yield return new WaitForSeconds(singleSpinDuration);
        }


        animator.enabled = false;

        if (wheelImage != null && idleSprite != null)
            wheelImage.sprite = idleSprite;

        Debug.Log("[WheelAnimation] Spin finished and reset to idle");

        currentSpin = null;
        onComplete?.Invoke();
    }
}