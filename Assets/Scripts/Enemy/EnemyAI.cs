using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;              // Speed of movement
    public float chaseRange = 5f;         // Distance within which the enemy will start chasing the player
    public float attackRange = 1.5f;      // Distance to trigger the attack
    public float attackCooldown = 2f;     // Time between attacks
    public Transform[] waypoints;         // List of points the enemy will randomly move towards
    public float waypointWaitTime = 2f;   // Time to wait at each waypoint before moving to the next one

    private Transform player;             // Reference to the player
    private Animator anim;
    private SpriteRenderer spriteRenderer;              
    private float nextAttackTime;         // Time to wait before the next attack
    private int currentWaypointIndex;     // Current waypoint the enemy is moving towards
    private bool isChasing = false;       // Whether the enemy is chasing the player

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // Find the player by its tag
        anim = GetComponent<Animator>();  // Get the Animator component
        currentWaypointIndex = Random.Range(0, waypoints.Length); // Start at a random waypoint
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= chaseRange)
        {
            // Start chasing the player if within range
            isChasing = true;
        }
        else
        {
            // Stop chasing and move to random waypoints if out of range
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    // Patrol between waypoints
    void Patrol()
    {
        // Move towards the current waypoint
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) > 0.2f)
        {
            MoveTowards(waypoints[currentWaypointIndex].position);
        }
        else
        {
            // Wait at the waypoint before moving to the next one
            anim.SetBool("isMoving", false);
            Invoke("ChangeWaypoint", waypointWaitTime);
        }
    }

    // Move towards a specific position
    void MoveTowards(Vector2 targetPosition)
    {
        anim.SetBool("isMoving", true); // Set the moving animation

    // Calculate the direction to the target, but keep the Y position unchanged
    Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
    
    // Lock the Y position to the current position
    Vector3 targetWithLockedY = new Vector3(targetPosition.x, transform.position.y, transform.position.z);

    // Flip the sprite to face the movement direction
    FlipSprite(direction);

    // Move the enemy, keeping the Y position the same
    transform.Translate(direction * speed * Time.deltaTime);

    // Optionally, update the position directly (if you need to directly set it without Translate)
    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Change to the next waypoint
    void ChangeWaypoint()
    {
        currentWaypointIndex = Random.Range(0, waypoints.Length);
    }

    // Chase the player when within range
    void ChasePlayer()
    {
        anim.SetBool("isMoving", true);
        MoveTowards(player.position);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            // Attack if within range of the player
            AttackPlayer();
        }
    }

    // Attack the player
    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("isAttacking");
            nextAttackTime = Time.time + attackCooldown;
            // Here you can add the logic for damaging the player
            Debug.Log("Enemy attacked the player!");
        }
    }

    // Animation event to reset attack trigger after attack is done
    public void FinishAttack()
    {
        // Reset attack state if needed
    }

    void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }
    }
}
