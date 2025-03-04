using UnityEngine;

public class AttackAreaScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if enemy collides with attack area
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy Hit!");
            //MAKE ENEMY TAKE DAMAGE
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.takeDamage();
            }
        }
    }
}
