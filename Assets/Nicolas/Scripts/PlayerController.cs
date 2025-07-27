using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f, sprintSpeed = 5f;
    [SerializeField] private GameObject faceEffect, lights;
    [SerializeField] private RuntimeAnimatorController controllerPresente, controllerPassado; //Referências para os controladores de animação do jogador no presente e passado
    private PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementVector;
    private Vector3 lastPosition;
    private bool wantsToSprint = false, canInteractAgain = true;
    public bool canMove = false, canRecoverStamina, isHiding=false;
    [HideInInspector] public bool canSprint = false, isSprinting = false, canTimeTravel = true;    //Flags de controle

    public LayerMask interacao; // Camada de interação
    private GameObject objetoInteracao, canvasObjInteracao;
    private RaycastHit2D hit = new RaycastHit2D();   //Raycast para detectar objetos
    private Stamina stamina;   //Referência ao sistema de stamina
    [SerializeField] private GameObject txtInteracao;   //Texto de interação

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
        //playerInputActions.Player.TimeTravel.performed += x => { if (canTimeTravel) TimeTravel(); };
        playerInputActions.Player.Pause.performed += x => GameControllerNicolas.GetInstance().PauseGame();

        movementVector = Vector2.zero; //Inicializa o vetor de movimento como zero
    }

    private void Interact()
    {    //Método para a interação
        bool isObjCasaPai = false;
        if (hit.collider != null)
        {
            if (canMove)
            {
                objetoInteracao = hit.collider.gameObject; //Armazena o objeto atingido pelo raycast
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
                else if (objetoInteracao.CompareTag("mesa") || objetoInteracao.CompareTag("armario"))
                {
                    isObjCasaPai = true;
                    canMove = false;
                    isHiding = true;
                    GameControllerNicolas.GetInstance().HidePlayer(); //Chama o método de esconder o jogador
                    lastPosition = transform.position;
                    //Debug.Log("Interagindo com: " + objetoInteracao.name);
                    Vector3 newPosition = new Vector3(objetoInteracao.transform.parent.transform.position.x, objetoInteracao.transform.parent.transform.position.y, transform.position.z);
                    transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    transform.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                    transform.position = newPosition;  //Move o jogador para dentro do objeto interagido
                }

                //canMove = false;
                canInteractAgain = false;
                txtInteracao.SetActive(false); // Desativa o texto de interação

                if (!isObjCasaPai)
                    objetoInteracao.SendMessage("interacao", new object[] { true, true }, SendMessageOptions.DontRequireReceiver);   //Envia a mensagem de interação para o objeto atingido pelo raycast
            }
        }
        else
        {
            if (isHiding)
            {
                canMove = true; // Permite o movimento novamente
                isHiding = false; // Desativa o estado de esconder
                GameControllerNicolas.GetInstance().ShowPlayer(); //Chama o método de mostrar o jogador
                SetIdleDirection(0);
                transform.position = lastPosition; // Restaura a posição original do jogador
                transform.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
                //Debug.Log("Saindo do esconderijo");
            }
            objetoInteracao = null;
        }
    }
/*
    private void TimeTravel()
    {
        if (canMove)
        {
            //Debug.Log("Time Travel");
            canMove = false; // Desabilita o movimento do jogador
            GameControllerNicolas.GetInstance().TimeTravel(); //Chama o método de viagem no tempo do controlador de jogo
        }
    }
*/

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

        if (!isHiding)
        {
            canRecoverStamina = canMove;
            hit = Physics2D.Raycast(rb.position, movementVector.normalized, 0.1f, interacao);
        }
        else
        {
            canRecoverStamina = true;
            hit = default;
        }
        //Debug.DrawRay(rb.position, movementVector.normalized * 1.0f, Color.blue); // Desenha um raio para depuração
        if (hit.collider != null)
        {
            //Debug.Log("Objeto interagível detectado: " + hit.collider.gameObject.name);
            objetoInteracao = hit.collider.gameObject;
            if (objetoInteracao.CompareTag("door"))
            {
                if (canInteractAgain)
                {
                    string txtInterac = GameControllerNicolas.GetInstance().GetInteractionText(hit.collider.gameObject);  //Obtém o texto de interação do objeto
                    txtInteracao.GetComponent<TMPro.TextMeshProUGUI>().text = txtInterac;
                    txtInteracao.SetActive(true); // Ativa o texto de interação
                }
            }
            else
            {
                if (objetoInteracao.transform.parent.transform.Find("canvas") != null)
                    canvasObjInteracao = objetoInteracao.transform.parent.transform.Find("canvas").gameObject; //Encontra o Canvas do objeto interagível
                if (canvasObjInteracao != null)
                    canvasObjInteracao.SetActive(true);    //Ativa o Canvas do objeto interagível
            }
        }
        else
        {
            if (canvasObjInteracao != null)
                canvasObjInteracao.SetActive(false); // Desativa o Canvas do objeto interagível
            txtInteracao.SetActive(false); // Desativa o texto de interação
            canInteractAgain = true;
        }


        if (stamina.CanRun(wantsToSprint) && movementVector != Vector2.zero)
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

    public void SetPlayerSprite(string time)
    {
        if (time == "Past")
        {
            animator.runtimeAnimatorController = controllerPassado;  //Muda o controlador de animação do jogador para o passado
            faceEffect.GetComponent<Animator>().SetBool("Present", false);
            lights.SetActive(false); // Desativa as luzes no passado
        }
        else if (time == "Present")
        {
            animator.runtimeAnimatorController = controllerPresente;  //Muda o controlador de animação do jogador para o presente
            faceEffect.GetComponent<Animator>().SetBool("Present", true);
            lights.SetActive(true); // Ativa as luzes no presente
        }
    }
    
    public void SetIdleDirection(int direction)
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        switch (direction)
        {
            case 0: //Frente
                animator.SetFloat("InputX", 0);
                animator.SetFloat("InputY", -1);
                break;
            case 1: //Direita
                animator.SetFloat("InputX", 1);
                animator.SetFloat("InputY", 0);
                break;
            case 2: //Esquerda
                animator.SetFloat("InputX", -1);
                animator.SetFloat("InputY", 0);
                break;
            case 3: //Trás
                animator.SetFloat("InputX", 0);
                animator.SetFloat("InputY", 1);
                break;
        }
        animator.SetFloat("LastInputX", animator.GetFloat("InputX"));
        animator.SetFloat("LastInputY", animator.GetFloat("InputY"));
    }
}
