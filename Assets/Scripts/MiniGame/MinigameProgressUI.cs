using UnityEngine;
using UnityEngine.UI;

public class MinigameProgressUI : MonoBehaviour
{
    public Slider progressSlider;

    private int lastTotal = -1;

    private void Awake()
    {
        progressSlider.minValue = 0;
        progressSlider.value = 0;
    }

    public void UpdateProgress()
    {
        if (GameProgressManager.Instance == null)
            return;

        int completed = GameProgressManager.Instance.GetCompletedMinigameCount();
        int total = GameProgressManager.Instance.GetTotalMinigameCount();

        if (total <= 0)
        {
            Debug.LogWarning("Total minigames not ready yet.");
            return;
        }

        if (total != lastTotal)
        {
            progressSlider.minValue = 0;
            progressSlider.maxValue = total;
            lastTotal = total;
        }

        progressSlider.value = completed;

        Debug.Log($"Progress UI refreshed: {completed}/{total}");
    }
}
