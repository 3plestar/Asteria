using UnityEngine;
using UnityEngine.EventSystems;


public class NoButtonUnfocussing : MonoBehaviour
{
    //this is a temporary fix
    GameObject lastselect;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }
}
