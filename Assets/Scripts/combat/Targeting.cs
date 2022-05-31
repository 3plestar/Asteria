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

    //Je code ziet er netjes uit. Ik denk dat het korter en duidelijker kan maar dat zijn kleine puntjes. 
    private void Start()
    {
        walkAnim = GetComponent<Animator>();
    }

    void Update()
    { 
        FocusTarget(currentTarget);  
    }
    //je zegt dat ongeacht of je een target hebt of niet dat je de hitcolliders wilt verzamelen? Hit colliders zijn toch ook possible targets? kan je dat niet samen voegen?
    //ik heb het gevoel dat deze vrij overdreven moeilijk is gemaakt. 

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
            //deze regel is overbodig. je gebruikt hem bijna niet en kan beter het gewoon aanroepen in de twee keer dat je hem gebruikt dan heb je weer een variabele minder nodig. 
            currentPosition = transform.position;
            reticle.transform.position = Camera.main.WorldToScreenPoint(target.position + Offset);
            
            Vector3 directionToTarget = target.position - currentPosition;
            walkAnim.SetFloat("horizontal", directionToTarget.x);
            walkAnim.SetFloat("vertical", directionToTarget.y);

            //waarom moet je door de hele hitcoolliders heen lopen om random isInRadius te zeggen? Ik denk dat je toch onderscheid moet maken met de Untarget functie en niet als een algemen uitweg.
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

    //Kleine typefout in direcion.
    public void cycleTarget(int direcion)
    {   //als je hier de list gebruikt die je eerder hebt gemaakt implaats van een nieuwe array kan je contains doen hoef je 1 for loop minder. Dit is niet persee efficienter maar wel een optie die een goed alternatief is.
        GameObject[] possibleTargets = GetPossibleTargets();
        int amountOfTargets = possibleTargets.Length - 1;
        if (amountOfTargets == 0 || !currentTarget)
        {
            return;
        }

        // als je hier de <= veranderd in < dan kan je de -1 van amountoftargets aflaten en klopt het getal beter.

        //first get the current target
        for (int i=0; i <= amountOfTargets; i++)
        {

            if(possibleTargets[i].transform == currentTarget)
            {
                nextTarget = i;
            }
        }
        
        //then cycle to direction
        nextTarget += direcion;

        //als je amount of targets niet de -1 geeft moet je hier >= doen
        if (nextTarget > amountOfTargets)
        {
            nextTarget = 0;
        }
        //als je amount of targets niet de -1 geeft moet je hier -1 wel doen for next target omdat dit naar index wijst.
        if (nextTarget < 0)
        {
            nextTarget = amountOfTargets;
        }

        currentTarget = possibleTargets[nextTarget].transform;
    }

    //als je hier toch all een list maakt waarom gebruik je die dan niet. nu maak je meerdere arrays voor het zelfde?
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

    //Untarget wordt aangeroepen als isInradius false is en vervolgens zet je is in radius op false. Dat klopt niet. 
    //Ik denk dat je untarget nu te extreem is. Je wilt eigenlijk alleen eentje untargeten als die out of range gaat of dood gaat. je called dit nu heel vaak en dat vereist heel veel interacties. Kijk eens naar on trigger enter.
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

