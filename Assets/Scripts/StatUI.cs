using UnityEngine;
using System.Collections.Generic;

public class MenuStatsController : MonoBehaviour
{
    [Header("UI References")]
    public Transform gridParent;
    public GameObject neighborStatsPrefab;

    private void OnEnable()
    {
        RefreshStats();
    }

    public void RefreshStats()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        List<MinigameSaveEntry> completed =
            GameProgressManager.Instance.GetCompletedMinigamesForSave();

        foreach (var entry in completed)
        {
            GameObject obj = Instantiate(neighborStatsPrefab, gridParent);
            NeighborStatsPrefab ui = obj.GetComponent<NeighborStatsPrefab>();
            ui.SetData(entry);
        }
    }
}
