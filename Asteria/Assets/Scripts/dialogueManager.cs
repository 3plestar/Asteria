using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    private responseManager ResponseManager;

    public bool isShown { get; private set; }
    public float textSpeed;

    private void Start()
    {
        ResponseManager = GetComponent<responseManager>();
        hideDialogueBox();
    }

    public void ShowDialogue(DialogueData dialogueData)
    {
        if (dialogueData != null)
        {
            isShown = true;
            dialogueBox.SetActive(true);
            StartCoroutine(NextDialogue(dialogueData));
        }
        else
        {
            hideDialogueBox();
        }
       
    }

    private IEnumerator NextDialogue(DialogueData dialogueData)
    {
        

        for (int i = 0; i < dialogueData.Dialogue.Length; i++)
        {
            string dialogue = dialogueData.Dialogue[i];
            yield return RunDialogue(dialogue, textLabel);

            if (i == dialogueData.Dialogue.Length - 1 && dialogueData.hasOptions) break;

            yield return new WaitUntil(() => Input.GetButton("Submit"));
        }

        if (dialogueData.hasOptions)
        {
            ResponseManager.NextResponse(dialogueData.TextOptions);
        }
        else
        {
            hideDialogueBox();
        }
        
    }

    private IEnumerator RunDialogue(string dialogue, TMPro.TMP_Text textLabel)
    {
        float t = 0;
        int charIndex = 0;

        while (charIndex < dialogue.Length)
        {
            t += Time.deltaTime * textSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, dialogue.Length);

            textLabel.text = dialogue.Substring(0, charIndex);
            yield return null;
        }
        textLabel.text = dialogue;

    }

    private void hideDialogueBox()
    {
        isShown = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
