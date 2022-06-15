using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public float strength = 2;
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other && other.CompareTag("Enemy"))
        {
            Vector3 directionToHit = other.transform.position - playerController.transform.position;
            other.GetComponent<EnemyStats>().takeDamage(strength,directionToHit);

        }
    }
}
