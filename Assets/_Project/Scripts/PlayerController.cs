using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Movement")]
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneChangeSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -20f;

    private CharacterController characterController;

    private Vector3 moveDirection;

    private int currentLane = 1;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleLaneInput();
        HandleMovement();
        HandleJump();
    }

    private void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentLane--;

            if (currentLane < 0)
            {
                currentLane = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentLane++;

            if (currentLane > 2)
            {
                currentLane = 2;
            }
        }
    }

    private void HandleMovement()
    {
        Vector3 targetPosition = transform.position;

        if (currentLane == 0)
        {
            targetPosition.x = -laneDistance;
        }
        else if (currentLane == 1)
        {
            targetPosition.x = 0;
        }
        else if (currentLane == 2)
        {
            targetPosition.x = laneDistance;
        }

        Vector3 moveVector = targetPosition - transform.position;

        moveDirection.x = moveVector.x * laneChangeSpeed;
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            moveDirection.y = -1f;

            if (
                Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow)
            )
            {
                moveDirection.y = jumpForce;
            }
        }
        else
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
}