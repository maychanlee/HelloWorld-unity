using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;

    [TextArea(2, 4)]
    public string text;

    [Header("Choice")]
    public bool hasYesNo;

    [Tooltip("Seconds to wait before auto-advancing (non-choice lines)")]
    public float autoAdvanceDelay = 2f;

    public int yesNextIndex = -1;
    public int noNextIndex = -1;
}
