using UnityEngine;

public class PlayerControle : MonoBehaviour
{
    public int velocidade = 6; // Velocidade de movimento do personagem

    private Rigidbody2D corpoRigidbody; // Referência ao Rigidbody2D do personagem
    private Vector2 direcaoMovimento;   // Direção do input do jogador (horizontal/vertical)
    private Vector2 velocidadeMovimento; // Velocidade convertida em vetor (X, Y)
    float multiplicadorVelocidade = 1f; // Multiplicador de velocidade para correr
    private bool estaCorrendo = false; // Flag para verificar se o personagem está correndo


    void Start()
    {
        // Define a velocidade nos eixos X e Y com base no valor da variável 'velocidade'
        velocidadeMovimento = new Vector2(velocidade, velocidade);

        // Pega o componente Rigidbody2D do objeto para controlar o movimento com física
        corpoRigidbody = GetComponent<Rigidbody2D>();
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
    }
}
