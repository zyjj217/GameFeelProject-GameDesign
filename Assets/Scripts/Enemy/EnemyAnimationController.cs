using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(bool moving)
    {
        animator.SetBool("isMoving", moving);
    }

    public void Attack()
    {
        animator.SetTrigger("isAttacking");
    }

    public void TakeHit()
    {
        animator.SetTrigger("isHit");
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
    }

    public void Jump(bool jumping)
    {
        animator.SetBool("isJumping", jumping);
    }
}
