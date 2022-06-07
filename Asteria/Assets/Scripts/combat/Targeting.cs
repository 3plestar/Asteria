using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject reticle;
    public Vector3 Offset;

    public Transform currentTarget;
    private int nextTarget;
    private int nextExtreme;

    [SerializeField] private List<GameObject> possibleTargets;

    private Animator walkAnim;

    private void Start()
    {
        walkAnim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        FocusTarget(currentTarget);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            possibleTargets.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.transform == currentTarget)
            {
                Untarget();
            }
            possibleTargets.Remove(other.gameObject);
        }
    }

    public void TargetObject()
    {
        if (!currentTarget)
        {
            currentTarget = GetClosestObject();
            return;
        }
        Untarget();
    }

    private Transform GetClosestObject()
    {
        Transform closestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject potentialTarget in possibleTargets.ToArray())
        {
            Vector3 directionToTarget = potentialTarget.transform.position - transform.position;
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
        return null;
    }

    public void FocusTarget(Transform target)
    {
        reticle.gameObject.SetActive(target);

        if (target)
        {
            reticle.transform.position = Camera.main.WorldToScreenPoint(target.position + Offset);

            Vector3 directionToTarget = target.position - transform.position;
            walkAnim.SetFloat("horizontal", directionToTarget.x);
            walkAnim.SetFloat("vertical", directionToTarget.y);
        }
    }

    public void cycleTarget(int direction)
    {
        if (!currentTarget)
        {
            return;
        }

        int amountOfTargets = possibleTargets.ToArray().Length;

        if (amountOfTargets == 1)
        {
            return;
        }

        //cycle to direction
        float nextTargetX = Mathf.Infinity;
        float possibleTargetX;

        for (int i = 0; i < amountOfTargets; i++)
        {
            possibleTargetX = possibleTargets[i].transform.position.x * direction;
            if (possibleTargetX < nextTargetX && possibleTargetX > currentTarget.position.x * direction)
            {
                nextTargetX = possibleTargetX;

                nextTarget = i;
            }
        }
        
        if(currentTarget != possibleTargets[nextTarget].transform)
        {
            currentTarget = possibleTargets[nextTarget].transform;
        }
        else
        {
            float extremeTargetX = Mathf.Infinity;
            for (int i = 0; i < amountOfTargets; i++)
            {
                possibleTargetX = possibleTargets[i].transform.position.x * direction;
                if (possibleTargetX < extremeTargetX)
                {
                    extremeTargetX = possibleTargetX;
                    nextExtreme = i;
                }
            }
            currentTarget = possibleTargets[nextExtreme].transform;
        }

        //first get the current target
        //for (int i = 0; i < amountOfTargets; i++)
        //{
        //    if (possibleTargets[i].transform == currentTarget)
        //    {
        //        nextTarget = i;
        //    }
        //}

        //then cycle to direction
        //nextTarget += direction;

        //if (nextTarget >= amountOfTargets)
        //{
        //    nextTarget = 0;
        //}

        //if (nextTarget < 0)
        //{
        //    nextTarget = amountOfTargets - 1;
        //}

        //currentTarget = possibleTargets[nextTarget].transform;
    }

    public void Untarget()
    {
        if (currentTarget)
            currentTarget = null;
    }
}