using UnityEngine;
//using UnityEngine.Events;
using UltEvents;

[System.Serializable]
public class OptionEvent
{
    [HideInInspector] public string name;
    [SerializeField] private UltEvent onPickedOption;

    public UltEvent OnPickedOption => onPickedOption;
}
