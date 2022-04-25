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
    public bool haltWalk { get; private set; }
    public float textSpeed;

    private void Start()
    {
        ResponseManager = GetComponent<responseManager>();
        StartCoroutine(hideDialogueBox());
    }

    public void ShowDialogue(DialogueData dialogueData)
    {
        if (dialogueData != null)
        {
            isShown = true;
            haltWalk = true;
            dialogueBox.SetActive(true);
            StartCoroutine(NextDialogue(dialogueData));
        }
        else
        {
            StartCoroutine(hideDialogueBox());
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
            StartCoroutine(hideDialogueBox());
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

    private IEnumerator hideDialogueBox()
    {
        Time.timeScale = 1;
        haltWalk = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        yield return new WaitForSeconds(1.0f);
        isShown = false;
    }
}
