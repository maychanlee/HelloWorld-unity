// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class TypewriterEffect : MonoBehaviour
// {
//     [SerializeField] private float typewriterSpeed = 30f;
//     public Coroutine Run(string textToType, TMP_Text textLabel)
//     {
//         return StartCoroutine(routine:TypeText(textToType, textLabel));
//     }
//     private IEnumerator TypeText(string textToType, TMP_Text textLabel)
//     {
//         textLabel.text = string.Empty;

//         float t = 0;
//         int charIndex = 0;

//         while(charIndex < textToType.Length)
//         {
//             t += Time.deltaTime;
//             charIndex = Mathf.FloorToInt(t * typewriterSpeed);
//             charIndex = Mathf.Clamp(value:charIndex, min:0, max:textToType.Length);

//             textLabel.text = textToType.Substring(startIndex:0, length:charIndex);

//             yield return null;
//         }

//         textLabel.text = textToType;
//     }
// }
