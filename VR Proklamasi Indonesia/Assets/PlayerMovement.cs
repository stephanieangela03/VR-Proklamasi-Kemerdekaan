using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public XRController leftController;
    public XRController rightController;

    private CharacterController characterController;
    private XROrigin xrRig;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        xrRig = GetComponent<XROrigin>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Get input from the right controller joystick
        Vector2 inputAxis;
        rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out inputAxis);

        // Get the forward and right vectors of the camera
        Transform cameraTransform = xrRig.Camera.transform;
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Make sure the forward vector is horizontal (y=0)
        cameraForward.y = 0f;
        cameraForward.Normalize();
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // Calculate the movement direction
        Vector3 moveDirection = (cameraForward * inputAxis.y + cameraRight * inputAxis.x).normalized;

        // Move the player
        characterController.Move(moveDirection * speed * Time.deltaTime);
    }
}
