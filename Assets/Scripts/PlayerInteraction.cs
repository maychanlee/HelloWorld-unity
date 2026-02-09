using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.Space;

    [Header("Icon")]
    public GameObject interactionIcon;

    private IInteractable currentInteractable;

    private void Start()
    {
        interactionIcon.SetActive(false);
    }

    private void Update()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            interactionIcon.SetActive(false);
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IInteractable>() == currentInteractable)
        {
            currentInteractable = null;
            interactionIcon.SetActive(false);
        }
    }
}
