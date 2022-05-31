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
        if(TryGetComponent(out DialogueOptionEvents optionEvents) && optionEvents.dialogueData == DialogueData)
        {
            player.dialogueManager.AddOptionEvents(optionEvents.Events);
        }

        player.dialogueManager.ShowDialogue(DialogueData);
    }
}
