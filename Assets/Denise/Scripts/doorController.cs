using System.Collections;
using UnityEngine;

public class doorController : MonoBehaviour
{
    private fadeController _fadeController; // Referência ao controlador de fade
    private PlayerController _playerController; // Referência ao controlador do jogador

    public Transform tPlayer; // Transform do player
    public Transform destino; // Ponto de destino para onde a porta levará o jogador
    [SerializeField] private Collider2D destinationBounds; //Referência ao objeto que define os limites de destino

    private bool isTransicionando; // Flag para verificar se a transição está em andamento

    public Vector2 idleDirection; // Direção padrão é para baixo (idle para baixo)


    void Start()
    {
        // Encontra o controlador de fade na cena
        _fadeController = FindAnyObjectByType<fadeController>();
        // Encontra o controlador do jogador na cena
        _playerController = FindAnyObjectByType<PlayerController>(); 
    }

    public void interacao()
    {
        // Inicia a coroutine para acionar a porta
        if (!isTransicionando) // Verifica se não está em transição
        {
            StartCoroutine(acionarPorta()); // Inicia a coroutine
        }
        
    }

    IEnumerator acionarPorta()
    {
        isTransicionando = true; // Define a flag de transição como verdadeira
        _fadeController.fadeIn(); // Inicia o fade in
        yield return new WaitWhile(() => _fadeController.fumeImage.color.a < 0.9f); // Espera até que o fade in esteja completo
        yield return new WaitForSeconds(0.2f); // Espera mais um pouco

        _playerController.enabled = false; // Desabilita o controlador do jogador para evitar movimento durante a transição
        tPlayer.position = destino.position; // Teleporta o jogador para o ponto de destino
        CameraController.GetInstance().SetNewBounds(destinationBounds); //Atualiza os limites da câmera para o novo destino

        // Aplica a direção de idle
        Animator playerAnimator = tPlayer.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            // Garante que o jogador fique na direção correta
            playerAnimator.SetFloat("InputX", idleDirection.x);  // Direção horizontal
            playerAnimator.SetFloat("InputY", idleDirection.y);  // Direção vertical

            // Reseta as animações de movimento
            playerAnimator.SetFloat("LastInputX", idleDirection.x);
            playerAnimator.SetFloat("LastInputY", idleDirection.y);

            // Reseta animações de movimento
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("isRunning", false);
        }
        yield return null; // Espera um frame para garantir que a posição foi atualizada antes de continuar

        yield return new WaitForSeconds(0.5f); // Espera um pouco antes de continuar
        _fadeController.fadeOut(); // Inicia o fade out

         yield return new WaitForSeconds(0.5f); // Espera um pouco para dar tempo para o jogador se estabilizar na nova posição
        _playerController.enabled = true; // Reabilita o controlador do jogador

        isTransicionando = false; // Define a flag de transição como falsa
    }
}
