using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NeighborStatsPrefab : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Phrase;

    public void SetData(MinigameSaveEntry entry)
    {
        Name.text = entry.neighborName;
        Phrase.text = entry.completionPhrase;
    }
}