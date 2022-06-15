using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueOptionEvents : MonoBehaviour
{
    [SerializeField] private DialogueData DialogueData;
    [SerializeField] private OptionEvent[] events;

    public DialogueData dialogueData => DialogueData;

    public OptionEvent[] Events => events;


    public void OnValidate()
    {
        if (DialogueData == null) return;
        if (DialogueData.TextOptions == null) return;
        if (events != null && events.Length == DialogueData.TextOptions.Length) return;

        if(events == null)
        {
            events = new OptionEvent[DialogueData.TextOptions.Length];
        }
        else
        {
            Array.Resize(ref events, DialogueData.TextOptions.Length);
        }

        for(int i = 0; i < DialogueData.TextOptions.Length; i++)
        {
            TextOptions textOptions = DialogueData.TextOptions[i];

            if(events[i] != null)
            {
                events[i].name = textOptions.ResponseText;
                continue;
            }

            events[i] = new OptionEvent() {name = textOptions.ResponseText};
        }
    }
}
