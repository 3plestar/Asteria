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
    
    //Code in dit script is zeer duidelijk. Wel opletten dat Scriptnamen met een hoofdletter beginnen.
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
        //Je kan hier ook gebruik maken van Time.deltaTime dit zal een smoother effect hebben bij een on regelmatige framerate.
        Vector3 lerpPosition = Vector3.Lerp(transform.position, finalPosition, cameraSpeed);
        transform.position = lerpPosition;
        
        lerpZoom = Mathf.Lerp(lerpZoom, zoom, cameraSpeed);
        Camera.main.orthographicSize = lerpZoom;
    }
}
