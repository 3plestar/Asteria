using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 cameraOffset;
    public Vector3 finalPosition;
    private Vector3 lerpPosition;

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
        lerpPosition = transform.position;
    }

    void FixedUpdate()
    {
        FollowingPlayer();
        
        lerpPosition = Vector3.Lerp(transform.position, finalPosition, cameraSpeed * Time.deltaTime);
        transform.position = lerpPosition;
        
        lerpZoom = Mathf.Lerp(lerpZoom, zoom, cameraSpeed * Time.deltaTime);
        Camera.main.orthographicSize = lerpZoom;
    }

    private void FollowingPlayer()
    {
        if (targeting.currentTarget == null)
        {
            finalPosition = player.position + cameraOffset;
        }
        else
        {
            finalPosition = (targeting.currentTarget.position - player.position) / 2 + player.position + cameraOffset;
        }
    }

    public void SetZoom(float newZoom)
    {
        zoom = newZoom;
    }

    public void SetPosition(Vector3 newPosition)
    {
        finalPosition = newPosition;
    }
}
