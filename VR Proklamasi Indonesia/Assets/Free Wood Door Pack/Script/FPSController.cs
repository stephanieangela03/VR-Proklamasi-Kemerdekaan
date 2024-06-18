using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterScript
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        CharacterController characterController;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        [HideInInspector]
        public bool canMove = true;

        void Start()
        {
            characterController = GetComponent<CharacterController>();

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Align character's forward direction with camera's forward direction
            transform.forward = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
        }

        void Update()
        {
            // Use keyboard input for movement
            float inputHorizontal = Input.GetAxis("Horizontal");
            float inputVertical = Input.GetAxis("Vertical");

            // Get the forward and right vectors of the camera
            Vector3 forward = playerCamera.transform.forward;
            Vector3 right = playerCamera.transform.right;

            // Ensure movement is only on the XZ plane
            forward.y = 0f;
            forward.Normalize();
            right.y = 0f;
            right.Normalize();

            // Calculate current speeds
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float speed = isRunning ? runningSpeed : walkingSpeed;

            // Update existing class-level moveDirection
            Vector3 moveDirectionXZ = (forward * inputVertical + right * inputHorizontal).normalized * speed;

            // Handle jumping
            if (characterController.isGrounded)
            {
                moveDirection = moveDirectionXZ; // Reset the XZ movement when grounded
                moveDirection.y = 0f; // Reset the Y movement when grounded

                if (Input.GetButton("Jump") && canMove)
                {
                    moveDirection.y = jumpSpeed;
                }
            }
            else
            {
                moveDirection = moveDirectionXZ + new Vector3(0, moveDirection.y, 0);
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }

        }
    }
}
