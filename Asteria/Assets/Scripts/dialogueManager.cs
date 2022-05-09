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
    public bool haltMovement { get; private set; }
    public bool isRunning { get; private set; }
    public float textSpeed;

    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>(){'.','!','?'}, 0.6f),
        new Punctuation(new HashSet<char>(){',',';',':'}, 0.3f )
    };

    private Coroutine runningCoroutine;

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
            haltMovement = true;
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
            yield return RunDialogue(dialogue);

            textLabel.text = dialogue;

            if (i == dialogueData.Dialogue.Length - 1 && dialogueData.hasOptions) break;

            yield return null;
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

    private IEnumerator RunDialogue(string dialogue)
    {
        Run(dialogue, textLabel);
        while (isRunning)
        {
            yield return null;
            if (Input.GetButton("Cancel"))
            {
                Stop();
            }
        }
    }

    public void Run(string dialogue, TMPro.TMP_Text textLabel)
    {
        runningCoroutine = StartCoroutine(TypeDialogue(dialogue, textLabel));
    }

    public void Stop()
    {
        StopCoroutine(runningCoroutine);
        isRunning = false;
    }

    private IEnumerator TypeDialogue(string dialogue, TMPro.TMP_Text textLabel)
    {
        isRunning = true;
        float t = 0;
        int charIndex = 0;

        while (charIndex < dialogue.Length)
        {
            int lastCharIndex = charIndex;

            t += Time.deltaTime * textSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, dialogue.Length);

            for(int i = lastCharIndex; i<charIndex; i++)
            {
                bool islast = i >= dialogue.Length-1;

                textLabel.text = dialogue.Substring(0, i+1);

                if(IsPunctuation(dialogue[i], out float waitTime) && !islast && !IsPunctuation(dialogue[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
            }
            yield return null;
        }
        isRunning = false;
    }

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach(Punctuation category in punctuations)
        {
            if (category.Punctuations.Contains(character))
            {
                waitTime = category.WaitTime;
                return true;
            }
        }
        waitTime = default;
        return false;
    }

    private IEnumerator hideDialogueBox()
    {
        Time.timeScale = 1;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        haltMovement = false;
        yield return new WaitForSeconds(1.0f);
        isShown = false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}
