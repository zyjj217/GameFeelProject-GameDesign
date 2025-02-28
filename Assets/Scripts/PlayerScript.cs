using System.Threading;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Animator animator;
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    public ParticleSystem smoke;

    //player health
    int health = 5;
    float timeSinceHit = 0f;

    //player physics variables
    Vector2 jumpForce = new Vector2(0, 5);
    int speed = 5;

    float attackTimer = 0f;
    float attackTime = 1f;

    //player animation flags
    bool attacking = false;
    bool jumping = false;
    bool hit = false;
    bool dead = false;
    bool moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        float xDirection = 0;
        if (timeSinceHit >= 2)
        {
            hit = false;
        }
        //get player horizontal input only if the player is not hit and not dead
        if (!dead && !hit)
        {
            xDirection = Input.GetAxis("Horizontal");
        }

        //flip player sprite in correct direction
        if (xDirection > 0)
        {
            moving = true;
            spriteRenderer.flipX = false;
            smoke.Play();

        }
        else if (xDirection < 0)
        {
            moving = true;
            spriteRenderer.flipX = true;
            smoke.Play();

        }
        else
        {
            moving = false;
        }

        //alter player velocity
        body.linearVelocity = new Vector3(xDirection * speed, body.linearVelocity.y);

        //if the player presses jump and they aren't already jumping let them jump
        if (Input.GetAxis("Vertical") > 0 && !jumping && !dead && !hit)
        {
            jumping = true;
            moving = false;
            body.AddForce(jumpForce, ForceMode2D.Impulse);
            smoke.Play();
        }

        //check for player attacking
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attacking = true;
        }
        if (attacking)
        {
            attack();
        }
        //increase time since hit
        timeSinceHit += Time.deltaTime;

        //update animation flags
        updateAnimationFlags();
    }

    void updateAnimationFlags()
    {
        animator.SetBool("Moving", moving);
        animator.SetBool("Jumping", jumping);
        animator.SetBool("Hit", hit);
        animator.SetBool("Attacking", attacking);
        animator.SetBool("Dead", dead);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player collides with floor
        if (collision.gameObject.CompareTag("Floor"))
        {
            jumping = false;
        }else if (collision.gameObject.CompareTag("Enemy")){        //if player collides with an enemy
            if(timeSinceHit >= 2)
            {
                takeDamage();
            }
        }

    }

    void takeDamage()
    {
        if (health > 0)
        {
            hit = true;
            health--;
            moving = false;
            timeSinceHit = 0;
        }
        else
        {
            dead = true;
        }
        
    }

    void attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackTime)
        {
            attacking = false;
            attackTimer = 0;
        }
    }
}
