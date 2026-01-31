using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowMouse : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 0.05f;

    private Animator animator;
    private Camera mainCam;

    void Awake()
    {
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    void Update()
    {
        // Mouse position in world space
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Difference ONLY on X axis
        float deltaX = mouseWorldPos.x - transform.position.x;
        float absDeltaX = Mathf.Abs(deltaX);

        if (absDeltaX > stopDistance)
        {
            // Direction: -1 = left, +1 = right
            float directionX = Mathf.Sign(deltaX);

            // Move only in X
            transform.Translate(Vector2.right * directionX * moveSpeed * Time.deltaTime);

            // Animation
            animator.SetBool("isWalking", true);
            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", 0f);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
