using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float movementSpeed = 3f, sprintSpeed = 5f;

    private PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Vector2 movementVector;
    private bool isSprinting = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.SprintStart.performed += x => isSprinting = true;
        playerInputActions.Player.SprintFinish.performed += x => isSprinting = false;
    }

    private void OnEnable() {
        playerInputActions.Enable();
    }

    void OnDisable() {
        playerInputActions.Disable();
    }

    private void Update() {
        movementVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        if (!isSprinting)    //Se não estiver correndo, use a velocidade de movimento normal
            rb.MovePosition(rb.position + movementVector * movementSpeed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position + movementVector * sprintSpeed * Time.fixedDeltaTime);
    }
}
