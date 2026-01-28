using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string[] dialogueLines;
    public bool[] autoProgressLines = new bool[] { true };
    public bool[] endDialogueLines = new bool[] { false };
    public float autoProgressDelay = 1.0f;
    public float typeSpeed = 0.03f;
    public DialogueChoice[] dialogueChoices;
    
    private void OnValidate()
    {
        if (dialogueLines != null)
        {
            if (autoProgressLines.Length != dialogueLines.Length)
            {
                bool[] newAutoProgressLines = new bool[dialogueLines.Length];
                
                // Copy existing values
                for (int i = 0; i < newAutoProgressLines.Length; i++)
                {
                    if (i < autoProgressLines.Length)
                    {
                        newAutoProgressLines[i] = autoProgressLines[i];
                    }
                    else
                    {
                        // Default value for new entries
                        newAutoProgressLines[i] = true;
                    }
                }
                
                autoProgressLines = newAutoProgressLines;
            }
            
            if (endDialogueLines.Length != dialogueLines.Length)
            {
                bool[] newEndDialogueLines = new bool[dialogueLines.Length];
                
                for (int i = 0; i < newEndDialogueLines.Length; i++)
                {
                    if (i < endDialogueLines.Length)
                    {
                        newEndDialogueLines[i] = endDialogueLines[i];
                    }
                    else
                    {
                        newEndDialogueLines[i] = false;
                    }
                }
                
                endDialogueLines = newEndDialogueLines;
            }
        }
    }
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public string[] dialogueChoices;
    public int[] nextDialogueIndices;
}