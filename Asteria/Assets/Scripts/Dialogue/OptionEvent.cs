using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OptionEvent
{
    [HideInInspector] public string name;
    [SerializeField] private UnityEvent onPickedOption;

    public UnityEvent OnPickedOption => onPickedOption;
}
