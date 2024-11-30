using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public GameObject markerPrefab; // Reference to the marker prefab
    public Animator animator; // Reference to the Animator component

    private GameObject currentMarker; // To keep track of the current marker instance

    public float normalSpeed = 3.5f; // Normal walking speed
    public float sprintSpeed = 7.0f; // Sprinting speed

    private bool isSprinting = false; // To track if sprinting is active
    private bool isStopped = false; // To track if the agent is stopped

    private float stopThreshold = 0.1f; // Threshold to consider the agent as stopped
    private float idleDelay = 0.2f; // Time to wait before switching to idle
    private float idleTimer = 0f; // Timer for idle state

    void Start()
    {
        agent.speed = normalSpeed; // Set initial speed to normal
    }

    void Update()
    {
        // Mouse click handling
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);

                // Handle marker instantiation
                if (currentMarker != null)
                {
                    Destroy(currentMarker);
                }
                currentMarker = Instantiate(markerPrefab, hit.point, Quaternion.Euler(90, 0, 0));

                // Enable walking animation
                animator.SetBool("isWalking", true);
                isStopped = false;
                idleTimer = 0f; // Reset idle timer
            }
        }

        // Sprint handling when Shift key is held down
        HandleSprinting();

        // Check movement status and stop agent if needed
        CheckMovement();
    }

    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && agent.velocity.sqrMagnitude > 0.01f)
        {
            if (!isSprinting)
            {
                agent.speed = sprintSpeed; // Increase speed to sprint speed
                animator.SetBool("isSprinting", true);
                isSprinting = true;
            }
        }
        else
        {
            if (isSprinting)
            {
                agent.speed = normalSpeed; // Reset speed to normal
                animator.SetBool("isSprinting", false);
                isSprinting = false;
            }
        }
    }

    void CheckMovement()
    {
        // Check if agent has reached the destination and stopped
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (agent.velocity.magnitude <= stopThreshold)
            {
                idleTimer += Time.deltaTime; // Increment idle timer
                if (idleTimer >= idleDelay && !isStopped)
                {
                    isStopped = true;
                    agent.isStopped = true; // Stop the NavMeshAgent
                    agent.velocity = Vector3.zero; // Ensure the velocity is zero
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", true); // Trigger idle animation
                }
            }
            else
            {
                idleTimer = 0f; // Reset idle timer if agent is moving slightly
            }
        }
        else
        {
            isStopped = false; // Agent is moving
            agent.isStopped = false; // Allow the agent to move
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            idleTimer = 0f; // Reset idle timer
        }
    }
}
