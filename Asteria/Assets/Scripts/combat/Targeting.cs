using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject reticle;
    public Vector3 Offset;

    private Vector3 currentPosition;
    public float TargetRadius;

    public Transform currentTarget;

    public List<GameObject> possibleTargets;
    int nextTarget;

    private Animator walkAnim;
    private Collider2D[] hitColliders;
    public bool isInRadius;

    private void Start()
    {
        walkAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cycleTarget();
            
        }
        FocusTarget(currentTarget);  
    }

    void FixedUpdate()
    {
        if (currentTarget)
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, TargetRadius);
            possibleTargets = new List<GameObject>();
            for (int i = 0; i < hitColliders.Length; i++)
            {
                Collider2D potentialTarget = hitColliders[i];

                if (potentialTarget.CompareTag("Enemy"))
                {
                    possibleTargets.Add(potentialTarget.gameObject);
                }
            }
        }
    }
    

    public void TargetObject()
    {
        if (!currentTarget)
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, TargetRadius);
            currentPosition = transform.position;
            currentTarget = GetClosestObject();
        }
        else
        {
            Untarget();
        }
    }

    private Transform GetClosestObject()
    {
        Transform closestTarget = null;
        
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider2D potentialTarget in hitColliders)
        {
            if (potentialTarget.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestTarget = potentialTarget.transform;
                }
            }
        }
        if (closestTarget)
        {
            return closestTarget;
        }
        else
        {
            Untarget();
            return null;
        }
        
    }

    public void FocusTarget(Transform target)
    {
        reticle.gameObject.SetActive(target);

        if (target)
        {
            currentPosition = transform.position;
            reticle.transform.position = Camera.main.WorldToScreenPoint(target.position + Offset);
            
            Vector3 directionToTarget = target.position - currentPosition;
            walkAnim.SetFloat("horizontal", directionToTarget.x);
            walkAnim.SetFloat("vertical", directionToTarget.y);

            foreach (Collider2D col in hitColliders)
            {
                if (col.transform && col.transform == target)
                {
                    isInRadius = true;
                    break;
                }
                else
                {
                    isInRadius = false;
                }
            }
            if (!isInRadius)
            {
                Untarget();
            }
        }
    }

    public void cycleTarget()
    {
        if (possibleTargets.ToArray().Length != 0 && currentTarget)
        {
            nextTarget++;
            if (nextTarget >= possibleTargets.ToArray().Length)
            {
                nextTarget = 0;
            }

            currentTarget = possibleTargets[nextTarget].transform;
        }
    }

    public void Untarget()
    {
        hitColliders = null;
        currentTarget = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       
        Gizmos.DrawWireSphere(transform.position, TargetRadius);
    }
}

