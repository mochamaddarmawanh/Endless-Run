using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Movement")]
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneChangeSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -20f;

    [Header("Turn")]
    [SerializeField] private float turnAngle = 50f;
    [SerializeField] private float turnSpeed = 8f;
    private float targetRotationY;
    private float targetRotationZ;

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
        HandleRotation();
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

            targetRotationZ = turnAngle * 0.5f;
            targetRotationY = -turnAngle;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentLane++;

            if (currentLane > 2)
            {
                currentLane = 2;
            }

            targetRotationZ = -turnAngle * 0.5f;
            targetRotationY = turnAngle;
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

    private void HandleRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(
            0,
            targetRotationY,
            targetRotationZ
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );

        targetRotationZ = Mathf.Lerp(
            targetRotationZ,
            0,
            turnSpeed * Time.deltaTime
        );

        targetRotationY = Mathf.Lerp(
            targetRotationY,
            0,
            turnSpeed * Time.deltaTime
        );
    }
}