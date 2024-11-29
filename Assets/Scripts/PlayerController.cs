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
                if (currentMarker != null)
                {
                    Destroy(currentMarker);
                }
                currentMarker = Instantiate(markerPrefab, hit.point, Quaternion.Euler(90, 0, 0));
                animator.SetBool("isWalking", true);
            }
        }

        // Sprint handling when Shift key is held down
        HandleSprinting();

        // Check movement status
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
        // Check if agent is actively moving
        if (agent.remainingDistance > agent.stoppingDistance || agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}