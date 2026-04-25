using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WheelAnimation : MonoBehaviour
{
    public Animator animator;
    public Image wheelImage;
    public Sprite idleSprite;

    [Header("Spin Timing")]
    // Put how long the spin should last in seconds.
    // Example: 0.64f = less than 1 second.
    // Example: 64f = 64 full seconds.
    public float spinDuration = 278f;

    // If this is true, the script tries to use the real length of the WheelSpin clip from Unity.
    // If this is false, the script uses the spinDuration number above.
    public bool useAnimationClipLength = true;

    // This must match the animation state name inside your Animator.
    public string spinStateName = "WheelSpin";

    private Coroutine currentSpin;

    private void Awake()
    {
        ResetToIdle();
    }

    // Puts the wheel back to the normal still image.
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

    // Roulette calls this when the player presses the spin button.
    public void SpinWheel(System.Action onComplete)
    {

        if (currentSpin != null)
            StopCoroutine(currentSpin);

        currentSpin = StartCoroutine(PlaySpinOnce(onComplete));
    }

    private IEnumerator PlaySpinOnce(System.Action onComplete)
    {

        if (animator == null)
            animator = GetComponent<Animator>();

        if (wheelImage == null)
            wheelImage = GetComponent<Image>();

        if (animator == null)
        {
            ResetToIdle();
            onComplete?.Invoke();
            yield break;
        }

        // Turn the Animator on and play the wheel spin animation from the start.
        animator.enabled = true;
        animator.Play(spinStateName, 0, 0f);

        // Wait one frame so Unity has time to enter the animation state.
        yield return null;

        float waitTime = spinDuration;

        // If enabled, use the animation clip length from Unity instead of the manual number.
        if (useAnimationClipLength)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            waitTime = stateInfo.length;
        }


         //The animation plays only one time. No loop. No cycles.
         yield return new WaitForSeconds(waitTime);

         //Stop the Animator and return to the still wheel image.
        ResetToIdle();


        currentSpin = null;
        onComplete?.Invoke();
    }
}
