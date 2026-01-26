using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;
    CinemachineConfiner confiner;
    [SerializeField] private Direction direction;
    private float positionOffset = 2f;
    [SerializeField] private Transform targetPosition;

    enum Direction { Up, Down, Left, Right, Teleport }

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = mapBoundary;
            UpdatePlayerPosition(collision.gameObject);
        }
    }
    
    private void UpdatePlayerPosition(GameObject player)
    {
        if(direction == Direction.Teleport)
        {
            player.transform.position = targetPosition.position;
            return;
        }

        Vector3 newPosition = player.transform.position;
        switch (direction)
        {
            case Direction.Up:
                newPosition.y += positionOffset;
                break;
            case Direction.Down:
                newPosition.y -= positionOffset;
                break;
            case Direction.Left:
                newPosition.x -= positionOffset;
                break;
            case Direction.Right:
                newPosition.x += positionOffset;
                break;
        }
        player.transform.position = newPosition;
    }

}
