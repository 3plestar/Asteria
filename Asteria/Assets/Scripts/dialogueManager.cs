using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    public bool isShown { get; private set; }

    public float textSpeed;

    private void Start()
    {
        hideDialogueBox();
    }

    public void ShowDialogue(DialogueData dialogueData)
    {
            isShown = true;
            dialogueBox.SetActive(true);
            StartCoroutine(NextDialogue(dialogueData));
    }

    private IEnumerator NextDialogue(DialogueData dialogueData)
    {
        textLabel.text = string.Empty;
        foreach (string dialogue in dialogueData.Dialogue)
        {
            float t = 0;
            int charIndex = 0;

            while (charIndex < dialogue.Length)
            {
                t += Time.deltaTime * textSpeed;
                charIndex = Mathf.FloorToInt(t);
                charIndex = Mathf.Clamp(charIndex, 0, dialogue.Length);

                textLabel.text = dialogue.Substring(0,charIndex);
                yield return null;
            }
            textLabel.text = dialogue;

            yield return new WaitUntil(() => Input.GetButton("Submit"));
        }
        hideDialogueBox();
    }

    private void hideDialogueBox()
    {
        isShown = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
