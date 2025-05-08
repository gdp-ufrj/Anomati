using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float movementSpeed = 3f, sprintSpeed = 5f;

    private PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementVector;
    private bool isSprinting = false;

    //interação com objetos
    public LayerMask interacao; // Camada de interação
    private GameObject objetoInteracao; // Objeto que será interagido

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Adição dos eventos de inputs:
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.SprintStart.performed += x => isSprinting = true;
        playerInputActions.Player.SprintFinish.performed += x => isSprinting = false;
        playerInputActions.Player.Interact.performed += x => Interact();

        movementVector = Vector2.zero; //Inicializa o vetor de movimento como zero
    }


    private void Interact(){    //Método para a interação
        // lógica para interagir com objetos no jogo
        RaycastHit2D hit = Physics2D.Raycast(rb.position, movementVector.normalized, 1.0f, interacao);
        //Debug.DrawRay(rb.position, movementVector.normalized * 1.0f, Color.blue); // Desenha um raio para depuração

        // Verifica se o raycast acertou algo
        if (hit.collider != null)
        {
            objetoInteracao = hit.collider.gameObject;

            if (objetoInteracao.CompareTag("door"))
            {
                doorController door = objetoInteracao.GetComponent<doorController>();
                if (door != null)
                {
                    door.tPlayer = this.transform;
                }
            }

            objetoInteracao.SendMessage("interacao", SendMessageOptions.DontRequireReceiver);
            // Envia a mensagem de interação para o objeto atingido pelo raycast
        }
        else
        {
            objetoInteracao = null;
        } 
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

    public void Move(){    //Este método vai controlar o movimento do jogador e a animação de acordo com o vetor de movimento
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
