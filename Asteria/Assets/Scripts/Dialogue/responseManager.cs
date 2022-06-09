using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResponseManager : MonoBehaviour
{
    [SerializeField] private RectTransform OptionsContainer;
    [SerializeField] private RectTransform OptionButton;

    private DialogueManager dialogueManager;
    private OptionEvent[] optionEvent;

    private List<GameObject> tempButtons = new List<GameObject>();

    private void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }

    public void addOptionEvents(OptionEvent[] optionEvent)
    {
        this.optionEvent = optionEvent;
    }

    public void NextResponse(TextOptions[] Options)
    {
        for(int i = 0; i < Options.Length; i++)
        {
            TextOptions option = Options[i];
            int optionIndex = i;

            GameObject optionButton = Instantiate(OptionButton.gameObject, OptionsContainer);
            optionButton.gameObject.SetActive(true);
            optionButton.GetComponentInChildren<TMP_Text>().text = option.ResponseText;
            optionButton.GetComponent<Button>().onClick.AddListener(() => OnSelected(option, optionIndex));

            tempButtons.Add(optionButton);
        }
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(OptionsContainer.GetChild(0).gameObject, new BaseEventData(eventSystem));

        OptionsContainer.gameObject.SetActive(true);
    }

    private void OnSelected(TextOptions option, int optionIndex)
    {
        OptionsContainer.gameObject.SetActive(false);

        foreach(GameObject button in tempButtons)
        {
            Destroy(button);
        }
        tempButtons.Clear();

        if (optionEvent != null && optionIndex <= optionEvent.Length)
        {
            optionEvent[optionIndex].OnPickedOption?.Invoke();
        }

        optionEvent = null;


        dialogueManager.ShowDialogue(option.DialogueData);
    }
} 
