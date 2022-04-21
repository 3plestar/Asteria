using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour, Interactable
{
    [SerializeField] private DialogueData DialogueData;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out playerController player))
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out playerController player))
        {
            if (player.Interactable is TriggerDialogue triggerDialogue && triggerDialogue == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(playerController player)
    {
        player.dialogueManager.ShowDialogue(DialogueData);
    }
}
