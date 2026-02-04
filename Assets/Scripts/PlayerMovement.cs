using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector2 input;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Read input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            input.y = 0;
        else
            input.x = 0;

        // Animation logic
        if (input != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Move smoothly
        transform.Translate(input * moveSpeed * Time.deltaTime);

        //Debug Log
        Debug.Log($"[PlayerMovement Update] instanceID={GetInstanceID()}, speed={moveSpeed}");
    }
}