// using UnityEngine;

// public class InteractionDetector : MonoBehaviour
// {
//     private IInteractable interactableInRange;

//     [SerializeField]
//     private GameObject interactionIcon;

//     [SerializeField]
//     private KeyCode interactKey = KeyCode.Space;

//     private void Start()
//     {
//         if (interactionIcon != null)
//             interactionIcon.SetActive(false);
//     }

//     private void Update()
//     {
//         if (interactableInRange == null)
//             return;

//         // Safety check in case interactability changes while in range
//         if (!interactableInRange.CanInteract())
//         {
//             ClearInteractable();
//             return;
//         }

//         if (Input.GetKeyDown(interactKey))
//         {
//             interactableInRange.Interact();
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (interactableInRange != null)
//             return; // Single interactable only

//         if (collision.TryGetComponent(out IInteractable interactable) &&
//             interactable.CanInteract())
//         {
//             interactableInRange = interactable;
//             if (interactionIcon != null)
//                 interactionIcon.SetActive(true);
//         }
//     }

//     private void OnTriggerExit2D(Collider2D collision)
//     {
//         if (collision.TryGetComponent(out IInteractable interactable) &&
//             interactable == interactableInRange)
//         {
//             ClearInteractable();
//         }
//     }

//     private void ClearInteractable()
//     {
//         interactableInRange = null;
//         if (interactionIcon != null)
//             interactionIcon.SetActive(false);
//     }
// }
