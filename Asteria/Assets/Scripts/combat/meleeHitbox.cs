using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeHitbox : MonoBehaviour
{
    public float strength = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStats>().takeDamage(strength);
        }
    }
}
