using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public float attackRange = 5.0f;
    public float attackDuration = 2.0f;
    public float idleCooldown = 2.0f;
    private float attackTimer = 0;
    private float cooldownTimer = 0;
    private Transform playerTransform;
    private NavMeshAgent agent;

    // Visual and audio cues
    public Material safeMaterial;
    public Material dangerousMaterial;
    private Renderer renderer;
    public ParticleSystem dangerParticles;
    public GameObject dangerAura;
    public AudioSource dangerSound;

    private enum State { Chasing, Idle, Cooldown }
    private State currentState = State.Idle;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        dangerAura.SetActive(false);
        dangerParticles.Stop();
        currentState = State.Idle;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case State.Chasing:
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    EnterCooldown();
                }
                else
                {
                    agent.SetDestination(playerTransform.position);
                }
                break;
            case State.Cooldown:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    ExitCooldown();
                }
                else
                {
                    agent.isStopped = true; // Ensure the agent doesn't move during cooldown
                }
                break;
            case State.Idle:
                if (distance < attackRange && cooldownTimer <= 0)
                {
                    StartChasing();
                }
                break;
        }
    }

    void StartChasing()
    {
        currentState = State.Chasing;
        attackTimer = attackDuration;
        animator.SetBool("isFighting", true);
        animator.SetBool("isTired", false);
        ApplyDangerState();
        agent.isStopped = false;
    }

    void EnterCooldown()
    {
        currentState = State.Cooldown;
        cooldownTimer = idleCooldown;
        animator.SetBool("isFighting", false);
        animator.SetBool("isTired", true);
        ApplySafeState();
    }

    void ExitCooldown()
    {
        currentState = State.Idle;
        animator.SetBool("isTired", false);
        ApplySafeState();
    }

    void ApplyDangerState()
    {
        renderer.material = dangerousMaterial;
        dangerAura.SetActive(true);
        if (!dangerParticles.isPlaying)
            dangerParticles.Play();
        if (!dangerSound.isPlaying)
            dangerSound.Play();
    }

    void ApplySafeState()
    {
        renderer.material = safeMaterial;
        dangerAura.SetActive(false);
        dangerParticles.Stop();
        dangerSound.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentState == State.Chasing)
            {
                // Change scene logic based on the current scene
                if (currentScene == "City")
                {
                    SceneManager.LoadScene("Past"); // Transition to Past when caught in City
                }
                else if (currentScene == "Past")
                {
                    SceneManager.LoadScene("Lose"); // Go to Lose scene when caught in Past
                }
            }
            else if (currentState == State.Cooldown)
            {
                if (currentScene == "City")
                {
                    Debug.Log("AI destroyed during cooldown.");
                    Destroy(gameObject); // Destroy the AI when touched during cooldown in City
                    SceneManager.LoadScene("Win"); // Transition to Past when caught in City
                }
                // No destruction in Past during cooldown
            }
        }
    }

    void OnDestroy()
    {
        if (dangerParticles != null && dangerParticles.isPlaying)
            dangerParticles.Stop();
        if (dangerSound != null)
            dangerSound.Stop();
        if (dangerAura != null)
            dangerAura.SetActive(false);
        Debug.Log("AI object and associated effects destroyed.");
    }
}
