using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f, sprintSpeed = 5f;
    [SerializeField] private GameObject faceEffect;  //Efeito de rosto do jogador

    private PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementVector;
    private bool wantsToSprint = false, canInteractAgain = true;

    public bool canMove = false;   //Flag para controlar se o jogador pode se mover ou não  (será setado como verdadeiro quando o fade out terminar)
    [HideInInspector] public bool canSprint = false, isSprinting = false, canTimeTravel = true;    //Flags de controle

    //interação com objetos
    public LayerMask interacao; // Camada de interação
    private GameObject objetoInteracao; // Objeto que será interagido

    private RaycastHit2D hit = new RaycastHit2D(); // Raycast para detectar objetos

    private Stamina stamina;   //Referência ao sistema de stamina

    [SerializeField] private GameObject txtInteracao; // Texto de interação

    private void Awake()
    {
        stamina = GetComponent<Stamina>(); // Obtém a referência ao componente Stamina
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = false; // Inicializa a flag de movimento como falsa

        //Adição dos eventos de inputs:
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.SprintStart.performed += x => { if (canSprint) wantsToSprint = true; };
        playerInputActions.Player.SprintFinish.performed += x => wantsToSprint = false;
        playerInputActions.Player.Interact.performed += x => Interact();
        playerInputActions.Player.TimeTravel.performed += x => { if (canTimeTravel) TimeTravel(); };
        playerInputActions.Player.Pause.performed += x => GameControllerNicolas.GetInstance().PauseGame();

        movementVector = Vector2.zero; //Inicializa o vetor de movimento como zero
    }

    private void Interact()
    {    //Método para a interação
        // Verifica se o raycast acertou algo
        if (hit.collider != null)
        {
            objetoInteracao = hit.collider.gameObject;

            bool canInteract = GameControllerNicolas.GetInstance().CanInteractWithObject(objetoInteracao);   //Verifica se o objeto pode ser interagido
            if (!canInteract)
            {
                Debug.Log("Interação não permitida com: " + objetoInteracao.name);
                return;
            }

            if (objetoInteracao.CompareTag("door"))
            {
                doorController door = objetoInteracao.GetComponent<doorController>();
                if (door != null)
                    door.tPlayer = this.transform;

                canMove = false;
            }

            //canMove = false;
            canInteractAgain = false;
            txtInteracao.SetActive(false); // Desativa o texto de interação
            objetoInteracao.SendMessage("interacao", true, SendMessageOptions.DontRequireReceiver);   //Envia a mensagem de interação para o objeto atingido pelo raycast
        }
        else
            objetoInteracao = null;
    }

    private void TimeTravel()
    {
        if (canMove)
        {
            //Debug.Log("Time Travel");
            canMove = false; // Desabilita o movimento do jogador
            GameControllerNicolas.GetInstance().TimeTravel(); //Chama o método de viagem no tempo do controlador de jogo
        }
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Update()
    {
        Move(); // Chama o método de movimento a cada frame
    }

    void FixedUpdate()
    {
        if (GerenciadorDeDialogos.GetInstancia().dialogoAtivo) return; // Se o diálogo estiver ativo, não move o jogador

        hit = Physics2D.Raycast(rb.position, movementVector.normalized, 0.1f, interacao);
        //Debug.DrawRay(rb.position, movementVector.normalized * 1.0f, Color.blue); // Desenha um raio para depuração
        if (txtInteracao != null)
        {
            if (hit.collider != null)
            {
                if (canInteractAgain)
                {
                    //Debug.Log("Objeto interagível detectado: " + hit.collider.gameObject.name);

                    string txtInterac = GameControllerNicolas.GetInstance().GetInteractionText(hit.collider.gameObject);  //Obtém o texto de interação do objeto
                    txtInteracao.GetComponent<TMPro.TextMeshProUGUI>().text = txtInterac;
                    txtInteracao.SetActive(true); // Ativa o texto de interação
                }
            }
            else
            {
                txtInteracao.SetActive(false); // Desativa o texto de interação
                canInteractAgain = true;
            }
        }

        if (stamina.CanRun(wantsToSprint))
        {
            isSprinting = true;
            rb.MovePosition(rb.position + movementVector * sprintSpeed * Time.fixedDeltaTime);
        }
        else
        {
            isSprinting = false;
            rb.MovePosition(rb.position + movementVector * movementSpeed * Time.fixedDeltaTime);
        }
    }

    public void Move()
    {    //Este método vai controlar o movimento do jogador e a animação de acordo com o vetor de movimento

        if (GerenciadorDeDialogos.GetInstancia().dialogoAtivo) return; // Se o diálogo estiver ativo, não move o jogador

        if (canMove)
            movementVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        else
            movementVector = Vector2.zero; // Para o movimento se não puder mover
        if (movementVector != Vector2.zero)
        {
            if (isSprinting)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }
            animator.SetFloat("InputX", movementVector.x);
            animator.SetFloat("InputY", movementVector.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetFloat("LastInputX", animator.GetFloat("InputX"));
            animator.SetFloat("LastInputY", animator.GetFloat("InputY"));
        }
        if (faceEffect != null && faceEffect.activeSelf)
        {
            bool lookingUp = animator.GetFloat("InputY") > 0.1f;
            faceEffect.GetComponent<Animator>().SetBool("LookingUp", lookingUp);
            faceEffect.GetComponent<Animator>().SetFloat("InputX", animator.GetFloat("InputX"));
            faceEffect.GetComponent<Animator>().SetFloat("InputY", animator.GetFloat("InputY"));
        }
        //Debug.Log(movementVector); // Debug para verificar o vetor de movimento
    }
    
    public void SetIdleDirection()
    {
        animator.SetFloat("InputX", 0);
        animator.SetFloat("InputY", -1);

        animator.SetFloat("LastInputX", 0);
        animator.SetFloat("LastInputY", -1);

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
}
