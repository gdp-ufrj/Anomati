using UnityEngine;

public class PlayerControle : MonoBehaviour
{
    public int velocidade = 6; // Velocidade de movimento do personagem

    private Rigidbody2D corpoRigidbody; // Referência ao Rigidbody2D do personagem
    private Vector2 direcaoMovimento;   // Direção do input do jogador (horizontal/vertical)
    private Vector2 velocidadeMovimento; // Velocidade convertida em vetor (X, Y)
    float multiplicadorVelocidade = 1f; // Multiplicador de velocidade para correr
    private bool estaCorrendo = false; // Flag para verificar se o personagem está correndo
    private Vector2 ultimaDirecao; // Última direção do movimento do personagem

    //interação com objetos
    public LayerMask interacao; // Camada de interação
    public GameObject objetoInteracao; // Objeto que será interagido


    void Start()
    {
        // Define a velocidade nos eixos X e Y com base no valor da variável 'velocidade'
        velocidadeMovimento = new Vector2(velocidade, velocidade);

        // Pega o componente Rigidbody2D do objeto para controlar o movimento com física
        corpoRigidbody = GetComponent<Rigidbody2D>();

        // Direção inicial do personagem
        ultimaDirecao = Vector2.down; 
    }

    void Update()
    {
        // Captura a entrada do teclado 
        direcaoMovimento = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical")   
        ).normalized; // Normaliza o vetor para evitar movimento diagonal mais rápido

        // Alternativa para correr: se o jogador pressionar Shift
        // Se o jogador pressionar Shift, inverte o estado de correr
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            estaCorrendo = !estaCorrendo; // Inverte o estado
            multiplicadorVelocidade = estaCorrendo ? 1.5f : 1f;
        }
        
        /*
        // Alternativa para correr: se o jogador quiser correr pressionando Shiff
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            multiplicadorVelocidade = 1.5f; // Está segurando: corre
        }
            else
        {
            multiplicadorVelocidade = 1f;   // Soltou: volta ao normal
        }
        */

        // Se pressionar a tecla E e o objeto não for nulo
        if(Input.GetKeyDown(KeyCode.E) && objetoInteracao != null) 
        {
            if (objetoInteracao.tag == "door")
            {
                 // Passa a referência do jogador para o objeto porta
                objetoInteracao.GetComponent<doorController>().tPlayer = this.transform;
            }
            
            // Chama a função de interação do objeto
            objetoInteracao.SendMessage("interacao", SendMessageOptions.DontRequireReceiver); 
        }
    }

    void FixedUpdate()
    {
        // Calcula a nova velocidade do personagem com base na direção e velocidade
        // O vetor de movimento é multiplicado pela velocidade e pelo multiplicador de velocidade
        Vector2 deslocamento = direcaoMovimento * velocidade * multiplicadorVelocidade * Time.fixedDeltaTime;
        corpoRigidbody.MovePosition(corpoRigidbody.position + deslocamento);

        // Calcula a nova posição do personagem
        Vector2 novaPosicao = corpoRigidbody.position + deslocamento;

        // Move o personagem 
        corpoRigidbody.MovePosition(novaPosicao);

        // Atualiza a última direção do movimento
        if (direcaoMovimento.sqrMagnitude > 0.1f) // Se o vetor de movimento não for nulo
        {
            ultimaDirecao = direcaoMovimento.normalized; // Atualiza a última direção
        }

        // Chama a função de interação a cada atualização de física
        Interagir(); 

        
    }

    void Interagir()
    {
        // lógica para interagir com objetos no jogo
        RaycastHit2D hit = Physics2D.Raycast(corpoRigidbody.position, ultimaDirecao, 1.0f, interacao);
        Debug.DrawRay(corpoRigidbody.position, ultimaDirecao * 1.0f, Color.blue); // Desenha um raio para depuração

        // Verifica se o raio atingiu algum objeto
        if (hit == true)
        {
            objetoInteracao = hit.collider.gameObject; // Armazena o objeto atingido
        }
        else
        {
            objetoInteracao = null; // Se não atingiu nada, define como nulo
        }
    }
}
