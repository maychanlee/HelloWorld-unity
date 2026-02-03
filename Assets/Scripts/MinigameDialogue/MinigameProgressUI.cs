using UnityEngine;

public class MinigameProgressUI : MonoBehaviour
{
    public UnityEngine.UI.Image progressFill;
    public TMPro.TextMeshProUGUI progressText;

    public void UpdateProgress()
    {
        if (GameProgressManager.Instance == null)
            return;

        int completed = GameProgressManager.Instance.GetCompletedMinigameCount();
        int total = GameProgressManager.Instance.GetTotalMinigameCount();

        float progress = total > 0 ? (float)completed / total : 0f;

        progressFill.fillAmount = progress;
        progressText.text = $"{completed}/{total}";
    }
}
