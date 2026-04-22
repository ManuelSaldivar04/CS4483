using System.Collections;
using UnityEngine;

public class BossSequenceController : MonoBehaviour
{
    [Header("Detection")]
    public float horizontalPadding = 0f;
    public float verticalPadding = 0f;

    [Header("Movement Settings (Walk State)")]
    public float walkDistance = 3f;
    public float walkSpeed = 2f;

    [Header("Animation Lengths (seconds)")]
    public float transitionLength = 1.5f;
    public float walkLength = 2f;
    public float attackLength = 2f;

    [Header("Delays")]
    public float transitionDelay = 0f;
    public float attackDelay = 0f;

    private Animator anim;
    private Transform player;
    private Camera mainCam;
    private bool sequenceStarted = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("State", 0);
    }

    void Start()
    {
        mainCam = Camera.main;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        else Debug.LogError("No Player found!");
    }

    void Update()
    {
        if (!sequenceStarted && IsBossInCameraView())
        {
            sequenceStarted = true;
            StartCoroutine(PlaySequence());
        }
    }

    bool IsBossInCameraView()
    {
        if (mainCam == null) return false;
        Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);
        return viewportPos.x >= 0 && viewportPos.x <= 1 &&
               viewportPos.y >= 0 && viewportPos.y <= 1 &&
               viewportPos.z > 0;
    }

    IEnumerator PlaySequence()
    {
        // 1. Transition
        anim.SetInteger("State", 1);
        yield return new WaitForSeconds(transitionLength);
        if (transitionDelay > 0) yield return new WaitForSeconds(transitionDelay);

        // 2. Walk + movement
        anim.SetInteger("State", 2);
        Vector3 direction = Vector3.down;
        //direction.y = 0;
        //if (direction.magnitude < 0.1f) direction = transform.right;
        Vector3 walkStartPos = transform.position;
        Vector3 walkTargetPos = walkStartPos + direction * walkDistance;
        float moveDuration = walkDistance / walkSpeed;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(walkStartPos, walkTargetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = walkTargetPos;

        // Wait for the walk animation to finish (if movement finished earlier)
        // yield return new WaitForSeconds(walkLength - moveDuration);
        float remainingWalk = walkLength - moveDuration;
        if (remainingWalk > 0) yield return new WaitForSeconds(remainingWalk);
        if (attackDelay > 0) yield return new WaitForSeconds(attackDelay);

        // 3. Attack
        anim.SetInteger("State", 3);
        yield return new WaitForSeconds(attackLength);

        // 4. Idle
        anim.SetInteger("State", 4);
    }
}