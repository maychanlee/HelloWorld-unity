using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 input;
    private Animator animator;
    public float stopDistance = 0.1f;
    [SerializeField] float maxSpeed = 10f;

    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
 // Get mouse position in world space
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Direction from player to mouse
        Vector2 direction = (mouseWorldPos - transform.position);
        float distance = direction.magnitude;

        // Normalize for consistent speed
        direction.Normalize();

        if (distance > stopDistance)
        {
            // Move player
            transform.Translate(direction * maxSpeed * Time.deltaTime);

            // Animation
            animator.SetBool("isWalking", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
