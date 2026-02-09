using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public List<DialogueLine> lines;
}
