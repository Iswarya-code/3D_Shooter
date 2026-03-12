using UnityEngine;
using UnityEngine.InputSystem;     //to access new input actions

public class BossMovement : MonoBehaviour
{
    CharacterController controller;
    PlayerInput playerInput;
    public float speed = 30f;

    public Transform cameraTransform;

    //Input actions
    public InputAction moveAction;

   

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        // Vector3 move = new Vector3(input.x, 0f, input.y);
        Vector3 move = cameraTransform.right * input.x + cameraTransform.forward * input.y;
        move.y = 0f;
        controller.Move(move * speed * Time.deltaTime);

        if(move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}
