using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour, Interactable
{
    [SerializeField] private DialogueData DialogueData;

    public void UpdateDialogueData(DialogueData dialogueData)
    {
        this.DialogueData = dialogueData;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            if (player.Interactable is TriggerDialogue triggerDialogue && triggerDialogue == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(PlayerController player)
    {
        foreach (DialogueOptionEvents optionEvents in GetComponents<DialogueOptionEvents>())
        {
            if(optionEvents.dialogueData == DialogueData)
            {
                player.DialogueManager.AddOptionEvents(optionEvents.Events);
                break;
            }
        }
        

        player.DialogueManager.ShowDialogue(DialogueData);
    }
}
