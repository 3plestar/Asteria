using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody2D rb;

    public Vector2 velocity;

    public float horizontal;
    public float vertical;
    public float speed;
    private Animator walkAnim;
 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walkAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        velocity = new Vector2(horizontal, vertical);
        rb.AddForce(speed * Time.deltaTime * Vector2.ClampMagnitude(velocity,1));

        //animation stuff
        if (velocity != Vector2.zero)
        {
            walkAnim.SetFloat("horizontal", velocity.x);
            walkAnim.SetFloat("vertical", velocity.y);
            walkAnim.SetBool("isWalking", true);
        }
        else
        {
            walkAnim.SetBool("isWalking", false);
        }

        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (Vector3)rb.velocity);
    }
}