using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "Data/Minigame Data")]
public class MinigameData : ScriptableObject
{
    [Header("Identity")]
    public int neighborId;
    public int minigameId;
    public string neighborName;
    public string completionPhrase;

    [Header("Config")]
    public WeedGameConfig weedConfig;

    [Header("Dialogue")]
    public DialogueSequence preGameDialogue;
    public DialogueSequence postGameDialogue;
    public DialogueSequence notEnoughHungerDialogue;
}
