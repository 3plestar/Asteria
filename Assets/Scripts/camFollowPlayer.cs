using UnityEngine;

public class camFollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 cameraOffset;
    public Vector3 finalPosition;

    public float zoom;
    private float lerpZoom;

    public float cameraSpeed = 0.1f;

    private Targeting targeting;

    // Start is called before the first frame update
    void Start()
    {
        targeting = player.GetComponent<Targeting>();
        lerpZoom = zoom;
        transform.position = player.position + cameraOffset;
    }

    void FixedUpdate()
    { 
        
        if (targeting.currentTarget == null)
        {
            finalPosition = player.position + cameraOffset;
        }
        else
        {
            finalPosition = (targeting.currentTarget.position - player.position)/2 + player.position + cameraOffset;
        }
        Vector3 lerpPosition = Vector3.Lerp(transform.position, finalPosition, cameraSpeed);
        transform.position = lerpPosition;
        
        lerpZoom = Mathf.Lerp(lerpZoom, zoom, cameraSpeed);
        Camera.main.orthographicSize = lerpZoom;
    }
}
