using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBounceScript : MonoBehaviour
{
    public GameObject bounceEffect;
    public GameObject dropDownEffect;

    public InputActionReference action;
    public InputActionReference action2;
    public float bounceStrength = 5f;
    public float secondJumpMultiplier = 1.5f;

    private CharacterController characterController;
    private Vector3 currentVelocity;
    private bool isBouncing;
    private bool wasGrounded;

    public Transform leftHand;
    public Transform rightHand;

    private int jumpCount;
    public int maxJumps = 2;
    public float maxVelocity = 20f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        action.action.Enable();
        action2.action.Enable();
    }


    void Update()
    {
        JumpCheck();

        if (isBouncing)
        {
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);

            characterController.Move(currentVelocity * Time.deltaTime);
            currentVelocity += Physics.gravity * Time.deltaTime;

            if (!wasGrounded && characterController.isGrounded && currentVelocity.y <= 0f)
            {
                Instantiate(dropDownEffect, transform.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));

                currentVelocity = Vector3.zero;
                isBouncing = false;
                jumpCount = 0;
            }
            else if (currentVelocity.y < 0f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
                {
                    characterController.Move(Vector3.down * hit.distance);
                    currentVelocity = Vector3.zero;
                    isBouncing = false;
                    jumpCount = 0;
                }
            }
        }

        wasGrounded = characterController.isGrounded;
    }

    private void JumpCheck()
    {
        if (!((action.action.WasPressedThisFrame() && action2.action.IsPressed()) || (action.action.IsPressed() && action2.action.WasPressedThisFrame()))) return;

        if (jumpCount >= maxJumps) return;

        Vector3 avgDir = (leftHand.forward + rightHand.forward) / 2f;
        avgDir.Normalize();

        Instantiate(bounceEffect, transform.position,
            transform.rotation * Quaternion.Euler(90f, 0f, 0f));

        float jumpForce = bounceStrength;

        if (jumpCount == 1)
            jumpForce *= secondJumpMultiplier;

        if (currentVelocity.y < 0)
            currentVelocity.y = 0f;

        currentVelocity = -avgDir * jumpForce;
        isBouncing = true;

        jumpCount++;
    }
}
