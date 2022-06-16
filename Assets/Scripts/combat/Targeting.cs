using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public GameObject reticle;
    public Vector3 Offset;

    public Transform currentTarget;
    private int nextTarget;
    private float angleBetween;

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

        foreach (GameObject potentialTarget in possibleTargets)
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

        //cycle around player
        float nextTargetAngle = 361;

        Vector3 centerBetweenTargets = Vector3.zero;
        for (var i = 0; i < amountOfTargets; i++)
        {
            centerBetweenTargets += possibleTargets[i].transform.position;
        }
        centerBetweenTargets /= amountOfTargets;

        Vector3 currentTargetDirection = currentTarget.position - centerBetweenTargets;
        Vector3 possibleTargetDirection;
        

        for (int i = 0; i < amountOfTargets; i++)
        {
            possibleTargetDirection = possibleTargets[i].transform.position - centerBetweenTargets;
            angleBetween = AngleClockwise(possibleTargetDirection.normalized, currentTargetDirection.normalized, Vector3.forward);
            if (angleBetween < nextTargetAngle && angleBetween>0)
            {
               nextTargetAngle = angleBetween;

               nextTarget = i;
            }
        }
        currentTarget = possibleTargets[nextTarget].transform;

        //float possibleTargetX;

        //for (int i = 0; i < amountOfTargets; i++)
        //{
        //    possibleTargetX = possibleTargets[i].transform.position.x * direction;
        //    if (possibleTargetX < nextTargetX && possibleTargetX > currentTarget.position.x * direction)
        //    {
        //        nextTargetX = possibleTargetX;

        //        nextTarget = i;
        //    }
        //}


        //if(currentTarget != possibleTargets[nextTarget].transform)
        //{
        //  currentTarget = possibleTargets[nextTarget].transform;
        //}
        //else//als je niet van target veranderd, verander naar de extreme aan de andere kant
        //{
        //    float extremeTargetX = Mathf.Infinity;
        //    for (int i = 0; i < amountOfTargets; i++)
        //    {
        //        possibleTargetX = possibleTargets[i].transform.position.x * direction;
        //        if (possibleTargetX < extremeTargetX)
        //        {
        //            extremeTargetX = possibleTargetX;
        //            nextExtreme = i;
        //        }
        //    }
        //    currentTarget = possibleTargets[nextExtreme].transform;
        //}


        ////first get the current target
        //for (int i = 0; i < amountOfTargets; i++)
        //{
        //    if (possibleTargets[i].transform == currentTarget)
        //    {
        //        nextTarget = i;
        //    }
        //}

        ////then cycle to direction
        //nextTarget += direcion;

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

    public static float AngleClockwise(Vector3 vec1, Vector3 forward, Vector3 axis)
    {
        Vector3 right;

        //clockwise
        right = Vector3.Cross(forward, axis);
        forward = Vector3.Cross(axis, right);

        //anti-clockwise
        //right = Vector3.Cross(axis, forward);
        //forward = Vector3.Cross(right, axis);
        float angle = Mathf.Atan2(Vector3.Dot(vec1, right), Vector3.Dot(vec1, forward)) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }

    public void Untarget()
    {
        if (currentTarget)
            currentTarget = null;
    }
}