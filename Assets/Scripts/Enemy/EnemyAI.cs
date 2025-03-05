using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float health = 3f;
    public float speed = 2f;              // Speed of movement
    public float chaseRange = 5f;         // Distance within which the enemy will start chasing the player
    public float attackRange = 0.1f;      // Distance to trigger the attack
    public float attackCooldown = 2f;     // Time between attacks    
    private GameObject playerGameObject;

    private Transform player;             // Reference to the player
    private Animator anim;
    private SpriteRenderer spriteRenderer;              
    private float nextAttackTime;         // Time to wait before the next attack
    private int currentWaypointIndex;     // Current waypoint the enemy is moving towards
    private bool isChasing = false;       // Whether the enemy is chasing the player

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindWithTag("Player").transform;  // Find the player by its tag
        anim = GetComponent<Animator>();  // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(health == 0)
        {
            Debug.Log("enemy died.");
            Die();
            return;
        }
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
    }


    // Chase the player when within range
    void ChasePlayer()
    {
        anim.SetBool("isMoving", true);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            // Attack if within range of the player
            AttackPlayer(playerGameObject);
        }
        else
        {
            MoveTowards(player.position);
        }
    }

    // Attack the player
    void AttackPlayer(GameObject target)
    {
        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("isAttacking");
            nextAttackTime = Time.time + attackCooldown;
            // Here you can add the logic for damaging the player
            // Debug.Log("Enemy attacked the player!");
            PlayerScript playerScript = target.GetComponent<PlayerScript>();
            if (playerScript != null)
            {
                playerScript.takeDamage();
                Debug.Log("Player took damage!");
            }
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

    public void takeDamage()
    {
        
        Debug.Log("Enemy took damage!1");
        if (health > 0)
        {
            health--;
        }
        else
        {
            Debug.Log("enemy died.");
            Die();
        }
    }

    private void Die()
    {
        anim.SetBool("isDead", true);
        Destroy(gameObject, 0.5f);

    }
}
