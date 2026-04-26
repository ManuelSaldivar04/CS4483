using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] wayPoints;
    private int currentWaypointIndex;
    private bool isWaiting;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        wayPoints = new Transform[waypointParent.childCount];

        for(int i = 0; i < waypointParent.childCount; i++)
        {
            wayPoints[i] = waypointParent.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeManager.isTimeStopped || isWaiting)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = wayPoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);
        animator.SetBool("isWalking", direction.magnitude > 0f);

        if(Vector2.Distance(transform.position,target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(waitTime);

        //if looping is enabled: increment currentWaypointIndex and wrap around if needed
        // if not looping: increment currentWaypointIndex but don't exceed last waypoint
        currentWaypointIndex = loopWaypoints ? (currentWaypointIndex + 1) % wayPoints.Length : Mathf.Min(currentWaypointIndex + 1, wayPoints.Length - 1);

        isWaiting = false;
    }
}
