using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject reticle;
    public Vector3 Offset;
    public float TargetRadius;

    private Vector3 currentPosition;
    public Transform currentTarget;

    private List<GameObject> possibleTargets;
    private int nextTarget;

    private Animator walkAnim;
    public Collider2D[] hitColliders;
    public bool isInRadius;

    private void Start()
    {
        walkAnim = GetComponent<Animator>();
    }

    void Update()
    { 
        FocusTarget(currentTarget);  
    }

    void FixedUpdate()
    {
        if (currentTarget)
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, TargetRadius);
        }
    }
    

    public void TargetObject()
    {
        if (!currentTarget)
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, TargetRadius);
            currentPosition = transform.position;
            currentTarget = GetClosestObject();
            return;
        }
        Untarget();
    }

    private Transform GetClosestObject()
    {
        Transform closestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;

        GameObject[] possibleTargets = GetPossibleTargets();

        foreach (GameObject potentialTarget in possibleTargets)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = potentialTarget.transform;
            }
        }

        if (closestTarget)
        {
            return closestTarget;
        }
        Untarget();
        return null;
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

    public void cycleTarget(int direcion)
    {
        GameObject[] possibleTargets = GetPossibleTargets();
        int amountOfTargets = possibleTargets.Length - 1;
        if (amountOfTargets == 0 || !currentTarget)
        {
            return;
        }

        //first get the current target
        for (int i=0; i < amountOfTargets; i++)
        {
            if(possibleTargets[i].transform == currentTarget)
            {
                nextTarget = i;
            }
        }

        //then cycle to direction
        nextTarget += direcion;

        if (nextTarget > amountOfTargets)
        {
            nextTarget = 0;
        }

        if (nextTarget < 0)
        {
            nextTarget = amountOfTargets;
        }

        currentTarget = possibleTargets[nextTarget].transform;
    }

    private GameObject[] GetPossibleTargets()
    {
        possibleTargets = new List<GameObject>();
        foreach (Collider2D potentialTarget in hitColliders)
        {
            if (potentialTarget.CompareTag("Enemy"))
            {
                possibleTargets.Add(potentialTarget.gameObject);
            }
        }
        return possibleTargets.ToArray();
    }

    public void Untarget()
    {
        isInRadius = false;
        if(hitColliders!=null)
            hitColliders = null;

        if (currentTarget)
            currentTarget = null;
        
        if (possibleTargets!=null)
            possibleTargets.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
       
        Gizmos.DrawWireSphere(transform.position, TargetRadius);
    }
}

