using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldInteraction : MonoBehaviour
{
    public GameObject interactIcon;
    private Vector2 boxSize = new Vector2(5f, 2f);
    public Rect box;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            checkInteraction();
    }

    public void showInteractIcon()
    {
        interactIcon.SetActive(true);
    }

    public void hideInteractIcon()
    {
        interactIcon.SetActive(false);
    }

    private void checkInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position + (Vector3)box.position, boxSize, 0, Vector2.zero);

        if(hits.Length > 0)
        {
            foreach(RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
        
    }
}
