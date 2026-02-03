using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "Data/Minigame Data")]
public class MinigameData : ScriptableObject
{
    public int neighborId;
    public int minigameId;

    public DialogueSequence preGameDialogue;   // NEW
    public DialogueSequence postGameDialogue;  // NEW
    public DialogueSequence notEnoughHungerDialogue;

}
