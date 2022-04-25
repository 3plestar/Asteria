using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class responseManager : MonoBehaviour
{
    [SerializeField] private RectTransform OptionsContainer;
    [SerializeField] private RectTransform OptionButton;

    private dialogueManager DialogueManager;

    private List<GameObject> tempButtons = new List<GameObject>();

    private void Start()
    {
        DialogueManager = GetComponent<dialogueManager>();
    }

    public void NextResponse(TextOptions[] Options)
    {
        foreach (TextOptions option in Options)
        {
            GameObject optionButton = Instantiate(OptionButton.gameObject, OptionsContainer);
            optionButton.gameObject.SetActive(true);
            optionButton.GetComponentInChildren<TMP_Text>().text = option.ResponseText;
            optionButton.GetComponent<Button>().onClick.AddListener(() => OnSelected(option));

            tempButtons.Add(optionButton);
        }
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(OptionsContainer.GetChild(1).gameObject, new BaseEventData(eventSystem));

        OptionsContainer.gameObject.SetActive(true);
    }

    private void OnSelected(TextOptions option)
    {
        OptionsContainer.gameObject.SetActive(false);

        foreach(GameObject button in tempButtons)
        {
            Destroy(button);
        }
        tempButtons.Clear();
        
        DialogueManager.ShowDialogue(option.DialogueData);
    }
} 
