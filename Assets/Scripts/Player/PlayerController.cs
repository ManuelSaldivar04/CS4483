using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Speed")]
    public float moveSpeed;

    [Header("Solid Objects")]
    public LayerMask solidObjectLayer;

    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    private bool playingFootsteps = false;
    public float footStepSpeed = 0.5f;
    // Grid position (in tile coordinates)
    private Vector2Int gridPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPos.x = Mathf.RoundToInt(transform.position.x - 0.5f);
        gridPos.y = Mathf.RoundToInt(transform.position.y - 0.5f);
    }

    private void Update()
    {
        if (TimeManager.isTimeStopped)
        {
            //stop the player movement animation and footsteps while game is paused
            animator.SetBool("isMoving", false);
            StopFootSteps();
            return;
        }

        if (!isMoving)
        {
            // Remove diagonal movement
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector2Int targetGridPos = gridPos + new Vector2Int((int)input.x, (int)input.y);
                Vector3 targetWorldPos = new Vector3(targetGridPos.x + 0.5f, targetGridPos.y + 0.5f, transform.position.z);

                if (IsWalkable(targetWorldPos))
                {
                    StartCoroutine(Move(targetWorldPos, targetGridPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);

        if(isMoving && !playingFootsteps)
        {
            StartFootSteps();
        }
        else if (!isMoving && playingFootsteps)
        {
            StopFootSteps();
        }
    }


    IEnumerator Move(Vector3 targetWorldPos, Vector2Int targetGridPos)
    {
        isMoving = true;

        while ((targetWorldPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure exact final position
        transform.position = targetWorldPos;
        gridPos = targetGridPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetWorldPos)
    {
        // Check collision at the target tile position
        return Physics2D.OverlapCircle(targetWorldPos, 0.3f, solidObjectLayer) == null;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void SetPosition(Vector2 newWorldPos)
    {
        gridPos.x = Mathf.RoundToInt(newWorldPos.x - 0.5f);
        gridPos.y = Mathf.RoundToInt(newWorldPos.y - 0.5f);
        transform.position = new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, transform.position.z);
    }
    
    void StartFootSteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootStep), 0f, footStepSpeed);
    }

    void StopFootSteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootStep));
    }
    void PlayFootStep()
    {
        SoundEffectManager.Play("Footstep",true);
    }
}