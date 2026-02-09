using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;

    [TextArea(2, 4)]
    public string text;

    [Header("Choice")]
    public bool hasYesNo;

    public int yesNextIndex = -1;
    public int noNextIndex = -1;
    public float autoAdvanceDelay = 2f;
}
