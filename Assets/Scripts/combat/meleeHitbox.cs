using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeHitbox : MonoBehaviour
{
    public float strength = 2;
    [SerializeField] private playerController playerController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && other.CompareTag("Enemy"))
        {
            Vector3 directionToHit = other.transform.position - playerController.transform.position;
            other.GetComponent<EnemyStats>().takeDamage(strength,directionToHit);

        }
    }
}
