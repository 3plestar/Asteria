using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueData : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private TextOptions[] responses;

    public string[] Dialogue => dialogue;

    public TextOptions[] TextOptions => responses;

    public bool hasOptions => TextOptions != null && TextOptions.Length > 0;
}
