using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DialogueMinigameTrigger : MonoBehaviour
{
    [SerializeField] private WeedGameController weedGame;

    public void PlayNeighbor1_Minigame2()
    {
        weedGame.StartWeedGame(1, 2);
    }
}
