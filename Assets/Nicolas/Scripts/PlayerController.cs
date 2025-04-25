using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float movementSpeed = 3f, sprintSpeed = 5f;

    private PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementVector;
    private bool isSprinting = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.SprintStart.performed += x => isSprinting = true;
        playerInputActions.Player.SprintFinish.performed += x => isSprinting = false;

        movementVector = Vector2.zero; //Inicializa o vetor de movimento como zero
    }

    private void OnEnable() {
        playerInputActions.Enable();
    }

    void OnDisable() {
        playerInputActions.Disable();
    }

    private void Update() {
        Move(); // Chama o método de movimento a cada frame
    }

    void FixedUpdate() {
        if (!isSprinting)    //Se não estiver correndo, use a velocidade de movimento normal
            rb.MovePosition(rb.position + movementVector * movementSpeed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position + movementVector * sprintSpeed * Time.fixedDeltaTime);
    }

    private void Move(){    //Este método vai controlar o movimento do jogador e a animação de acordo com o vetor de movimento
        movementVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        if (movementVector != Vector2.zero) {
            if (isSprinting) {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            } 
            else {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }
            animator.SetFloat("InputX", movementVector.x);
            animator.SetFloat("InputY", movementVector.y);
        }
        else {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetFloat("LastInputX", animator.GetFloat("InputX"));
            animator.SetFloat("LastInputY", animator.GetFloat("InputY"));
        }

        //Debug.Log(movementVector); // Debug para verificar o vetor de movimento
    }
}
