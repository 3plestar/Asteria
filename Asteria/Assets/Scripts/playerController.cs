using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody2D rb;

    public Vector2 velocity;

    private float horizontal;
    private float vertical;
    public float speed;
    private Animator walkAnim;

    [SerializeField] private dialogueManager DialogueManager;

    public dialogueManager dialogueManager => DialogueManager;

    public Interactable Interactable { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walkAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isShown)
        {
            horizontal = 0;
            vertical = 0;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Submit"))
        {
            Interactable?.Interact(this);
        }
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