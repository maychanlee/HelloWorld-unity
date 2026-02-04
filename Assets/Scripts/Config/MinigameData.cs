using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "Data/Minigame Data")]
public class MinigameData : ScriptableObject
{
    public int neighborId;
    public string neighborName;
    public int minigameId;
    public string completionPhrase;

    public DialogueSequence preGameDialogue;
    public DialogueSequence postGameDialogue;
    public DialogueSequence notEnoughHungerDialogue;

}
