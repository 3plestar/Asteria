using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueOptionEvents))]

public class OptionEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueOptionEvents optionEvents = (DialogueOptionEvents)target;

        if (GUILayout.Button("Refresh"))
        {
            optionEvents.OnValidate();
        }
    }
}
