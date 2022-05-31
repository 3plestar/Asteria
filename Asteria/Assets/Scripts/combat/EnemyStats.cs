using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float takenKnockback;

    private Rigidbody2D rb;

    void Start()
    {
        health = maxHealth;
        rb = transform.GetComponent<Rigidbody2D>();
    }

    public void takeDamage(float damage,Vector2 knockbackDirection)
    {
        //damage for knockback scaling
        rb.AddForce(takenKnockback * Vector2.ClampMagnitude(knockbackDirection,1),ForceMode2D.Impulse);

        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
