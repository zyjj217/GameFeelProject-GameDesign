using System.Threading;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Animator animator;
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    public GameObject attackArea;
    public ParticleSystem smoke;
    public bool smokeOn = true;
    public bool screenShakeOn = true;
    public bool itemEffectsOn = true;
    public HealthBarScript healthBar;
    public GameObject heartPopupPrefab;
    public bool gameOver = false;
    private bool canDoubleJump = false; // figured since we got the sprites why not try it

    //player health
    public int health = 5;
    float timeSinceHit = 0f;
    float deadTime = 0f;

    //player physics variables
    Vector2 jumpForce = new Vector2(0, 5);
    int speed = 5;

    //player attack variables
    float attackTimer = 0f;
    float attackTime = 0.5f;

    //player animation flags
    bool attacking = false;
    bool jumping = false;
    bool hit = false;
    bool dead = false;
    bool moving = false;
    bool facingRight = true;

    //player screen shake effect
    private ScreenShake screenShake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackArea.SetActive(false);
        screenShake = Camera.main.GetComponent<ScreenShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            body.linearVelocity = new Vector3(0, 0, 0);
            return;
        }
        float xDirection = 0;
        if (health == 0 && deadTime >= 0.5f)
        {
            SceneManager.LoadScene("Map");
        }else if (health == 0) // if player is dead update animations flags and return
        {
            moving = false;
            attacking = false;
            hit = false;
            dead = true;
            deadTime += Time.deltaTime;
            updateAnimationFlags();
            return;
        }
        if (timeSinceHit >= 0.5)
        {
            hit = false;
        }
        //get player horizontal input only if the player is not hit and not dead
        if (!dead && !hit)
        {
            xDirection = Input.GetAxis("Horizontal");
        }

        //flip player sprite in correct direction
        if (xDirection != 0)
        {
            moving = true;
            flip(xDirection);
            if (smokeOn)
            {
                smoke.Play();
            }
        }else
        {
            moving = false;
        }

        //alter player velocity
        body.linearVelocity = new Vector3(xDirection * speed, body.linearVelocity.y);

        //if the player presses jump and they aren't already jumping let them jump
        // if (Input.GetAxis("Vertical") > 0 && !jumping && !dead && !hit)
        // {
        //     jumping = true;
        //     moving = false;
        //     body.AddForce(jumpForce, ForceMode2D.Impulse);
        //     if (smokeOn)
        //     {
        //         smoke.Play();
        //     }
        // }
        if (Input.GetKeyDown(KeyCode.W) && !dead && !hit)
        {
            if (!jumping) 
            {
                // First jump
                jumping = true;
                canDoubleJump = true;
                body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
                body.AddForce(jumpForce, ForceMode2D.Impulse);
            }
            else if (canDoubleJump)
            {
                // Second jump (double jump)
                canDoubleJump = false; // Disable further jumping
                body.linearVelocity = new Vector2(body.linearVelocity.x, 0); // Reset velocity then up
                body.AddForce(jumpForce, ForceMode2D.Impulse);

                animator.SetTrigger("DoubleJump"); 
            }
        }

        // Note: attempt to fix player animation, rewrite attacking logic
        //check for player attacking
        if (Input.GetKeyDown(KeyCode.Space) && !attacking && !hit)
        {
        //     moving = false;
        //     attacking = true;
        //     attackArea.SetActive(true);
        //     Debug.Log("Triggered Attack");
        // }
        // if (attacking)
        // {
        //     moving = false;
        //     jumping = false;
        //     attack();
        // }
        // if (jumping)
        // {
        //     moving = false;
        //     attacking = false;
            StartCoroutine(PerformAttack());
        }
        //increase time since hit
        timeSinceHit += Time.deltaTime;

        //update animation flags
        updateAnimationFlags();
    }

    IEnumerator PerformAttack()
    {
        attacking = true;
        attackArea.SetActive(true);
        animator.SetTrigger("Attacking"); // Use a trigger instead of a bool

        yield return new WaitForSeconds(attackTime); // Wait for the full attack animation

        attacking = false;
        attackArea.SetActive(false);
    }


    void updateAnimationFlags()
    {
        animator.SetBool("Moving", moving);
        animator.SetBool("Jumping", jumping);
        animator.SetBool("Hit", hit);
        //animator.SetBool("Attacking", attacking);
        animator.SetBool("Dead", dead);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player collides with floor
        if (collision.gameObject.CompareTag("Floor"))
        {
            jumping = false;
            updateAnimationFlags();
        }

    }

    public void takeDamage()
    {
        if (health > 0)
        {
            hit = true;
            if (screenShake != null && screenShakeOn)
            {
                screenShake.TriggerShake(0.2f, 0.3f); 
            }
            health--;
            moving = false;
            attacking = false;
            timeSinceHit = 0;
        }
        else
        {
            moving = false;
            attacking = false;
            hit = false;
            dead = true;
        }
        healthBar.setHealth(health);


    }

    void attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackTime)
        {
            attacking = false;
            attackTimer = 0;
            attackArea.SetActive(false);
        }
    }

    public void toggleSmoke()
    {
        smokeOn = !smokeOn;
    }
    public void toggleScreenShake()
    {
        screenShakeOn = !screenShakeOn;
    }
    public void toggleItemEffects()
    {
        itemEffectsOn = !itemEffectsOn;
    }
    void flip(float xDirection)
    {
        if(xDirection <0 && facingRight || xDirection > 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
public void OnHealthPickup()
    {
        if (itemEffectsOn)
        {
            // Instantiate the heart popup slightly above the player
            if (heartPopupPrefab != null)
            {
                Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);
                Instantiate(heartPopupPrefab, spawnPos, Quaternion.identity);
            }
            // Trigger a glow effect on the health bar
            if (healthBar != null)
            {
                healthBar.GlowEffect();
            }
        }
    }

    // Increase player's health and update the health bar
    public void IncreaseHealth(int amount)
    {
        health += amount;
        if (health > 5)
            health = 5;
        healthBar.setHealth(health);
    }
}
