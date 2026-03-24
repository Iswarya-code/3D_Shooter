using UnityEngine;
using UnityEngine.InputSystem;     //to access new input actions

public class BossMovement : MonoBehaviour
{
    CharacterController controller;
    PlayerInput playerInput;
    public float speed = 30f;

    float yVelocity = 0f;           //stores vertical movement
    float gravity = -9.81f;         // gravity value [negative = downward force]

    public Transform cameraTransform;

    //Input actions
    public InputAction moveAction;
    public InputAction lookAction;

    public float mouseSensitivity = 100f;
    float xRotation = 0f;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }
    // Update is called once per frame
    void Update()
    {


        Vector2 input = moveAction.ReadValue<Vector2>();
        // Vector3 move = new Vector3(input.x, 0f, input.y);
        Vector3 move = cameraTransform.right * input.x + cameraTransform.forward * input.y;

        //adding gravity
        if(controller.isGrounded && yVelocity <0)
        {
            yVelocity = -2f;       // Small negative value keeps player stuck to ground (prevents bouncing)
        }

        yVelocity += gravity * Time.deltaTime;       // Apply gravity every frame (makes player fall smoothly)

        // ----------- FINAL MOVEMENT -----------
        Vector3 finalMove = move * speed;          // Apply speed only to horizontal movement

        finalMove.y = yVelocity;              // Add vertical movement (gravity) separately

        controller.Move(finalMove * Time.deltaTime);         // Move player using CharacterController



        Vector2 look = lookAction.ReadValue<Vector2>();
        float mouseX = look.x * mouseSensitivity * Time.deltaTime;
        float mouseY = look.y * mouseSensitivity * Time.deltaTime;

        //vertical (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //horizontal (player)
        transform.Rotate(Vector3.up * mouseX);

    }
    
}
