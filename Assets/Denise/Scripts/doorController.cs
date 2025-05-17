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

        _playerController.canMove = false; // Desabilita o movimento do jogador
        tPlayer.position = destino.position; // Teleporta o jogador para o ponto de destino
        CameraController.GetInstance().SetNewBounds(destinationBounds); //Atualiza os limites da câmera para o novo destino

        _playerController.SetIdleDirection();   //Define a direção de idle do jogador

        yield return new WaitForSeconds(0.5f); // Espera um pouco antes de continuar
        _fadeController.fadeOut(); // Inicia o fade out

         yield return new WaitForSeconds(0.5f); // Espera um pouco para dar tempo para o jogador se estabilizar na nova posição
        _playerController.canMove = true; //Habilita o movimento do jogador novamente

        isTransicionando = false; // Define a flag de transição como falsa
    }
}
