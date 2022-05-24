using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody2D rb;
    Targeting targeting;

    public Vector2 velocity;

    private float horizontal;
    private float vertical;
    public float speed;
    private Animator walkAnim;
    public Animator attackAnim;
    public GameObject weapon;

    [SerializeField] private dialogueManager DialogueManager;
    

    public dialogueManager dialogueManager => DialogueManager;

    public Interactable Interactable { get; set; }
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walkAnim = GetComponent<Animator>();
        targeting = GetComponent<Targeting>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //interaction with interactable
        if (Input.GetButtonDown("Submit") && dialogueManager.isShown == false)
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
                targeting.Untarget();
            }
        }

        //target closest targetable object and cycle through
        if (Input.GetButtonDown("Target"))
        {
            targeting.TargetObject();
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            targeting.cycleTarget(1);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            targeting.cycleTarget(-1);
        }


        if (Input.GetButtonDown("Attack"))
        {
            weapon.SetActive(true);
            attackAnim.Play("attack");
            
        }
    }

    void FixedUpdate()
    {
        if (dialogueManager.isShown)
        {
            walkAnim.SetBool("isWalking", false);
            return;
        }

        velocity = new Vector2(horizontal, vertical);
        rb.AddForce(speed * Time.deltaTime * Vector2.ClampMagnitude(velocity,1));

        //animation stuff
        if (velocity != Vector2.zero)
        {
            walkAnim.SetBool("isWalking", true);
            if (targeting.currentTarget != null)
            {
                return;
            }
            walkAnim.SetFloat("horizontal", velocity.x);
            walkAnim.SetFloat("vertical", velocity.y);
        }
        else
        {
            walkAnim.SetBool("isWalking", false);
        }

        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + (Vector3)rb.velocity);
    }
}