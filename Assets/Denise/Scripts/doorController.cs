using System.Collections;
using System.IO;
using UnityEngine;

public class doorController : MonoBehaviour
{
    private fadeController _fadeController; // Referência ao controlador de fade

    public Transform tPlayer; // Transform do player
    public Transform destino; // Ponto de destino para onde a porta levará o jogador
    [SerializeField] private Collider2D destinationBounds; //Referência ao objeto que define os limites de destino


    void Start()
    {
        // Encontra o controlador de fade na cena
        _fadeController = FindAnyObjectByType<fadeController>();
    }

    public void interacao(bool isDoor=true)
    {
        // Inicia a coroutine para acionar a porta
        if (!DoorTransitionController.GetInstance().isTransitioning) // Verifica se não está em transição
        {
            StartCoroutine(acionarPorta(isDoor)); // Inicia a coroutine
        }
        
    }

    IEnumerator acionarPorta(bool isDoor=true)
    {
        DoorTransitionController.GetInstance().isTransitioning = true; // Define a flag de transição como verdadeira
        GameControllerNicolas.GetInstance().DisablePlayerMovement(); // Desabilita o movimento do jogador
        _fadeController.fadeIn(); // Inicia o fade in
        yield return new WaitWhile(() => _fadeController.fumeImage.color.a < 0.9f); // Espera até que o fade in esteja completo
        yield return new WaitForSeconds(0.2f); // Espera mais um pouco

        tPlayer.position = destino.position; // Teleporta o jogador para o ponto de destino
        CameraController.GetInstance().SetNewBounds(destinationBounds); //Atualiza os limites da câmera para o novo destino

        string origin = "", destination = "";
        origin = transform.parent.transform.parent.gameObject.name;   //Armazena o nome do cenário ond está a porta (a cena de origem)
        destination = destinationBounds.transform.parent.gameObject.name; //Armazena o nome do cenário de destino (a cena para onde a porta leva)
        GameControllerNicolas.GetInstance().BetweenDoorInteraction(origin, destination, isDoor);    //Cuida das coisas entre a transição de portas (como triggers, etc...)

        yield return new WaitForSeconds(0.5f); // Espera um pouco antes de continuar
        _fadeController.fadeOut(); // Inicia o fade out

        yield return new WaitForSeconds(0.5f); // Espera um pouco para dar tempo para o jogador se estabilizar na nova posição
        GameControllerNicolas.GetInstance().FinishDoorInteraction(origin, destination, isDoor);    //Finaliza a interação com a porta no controlador do jogo (ativa cenas, triggers, etc...)
    }
}
