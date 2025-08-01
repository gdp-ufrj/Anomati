using System.Collections;
using UnityEngine;

public class doorController : MonoBehaviour
{
    private fadeController _fadeController; // Referência ao controlador de fade

    public Transform tPlayer; // Transform do player
    public Transform destino; // Ponto de destino para onde a porta levará o jogador
    public int playerIdleDirection = 3;
    [SerializeField] private Collider2D destinationBounds; //Referência ao objeto que define os limites de destino


    void Start()
    {
        // Encontra o controlador de fade na cena
        _fadeController = FindAnyObjectByType<fadeController>();
    }

    public void interacao(object[] array)
    {
        bool isDoor = (bool)array[0];
        bool haveFade = (bool)array[1];
        // Inicia a coroutine para acionar a porta
        if (!DoorTransitionController.GetInstance().isTransitioning) // Verifica se não está em transição
        {
            StartCoroutine(acionarPorta(isDoor, haveFade)); // Inicia a coroutine
        }
        
    }

    IEnumerator acionarPorta(bool isDoor=true, bool haveFade=true)
    {
        DoorTransitionController.GetInstance().isTransitioning = true; // Define a flag de transição como verdadeira
        GameControllerNicolas.GetInstance().DisablePlayerMovement(); // Desabilita o movimento do jogador

        if (haveFade)
        {
            _fadeController.fadeIn(); // Inicia o fade in
            yield return new WaitWhile(() => _fadeController.fumeImage.color.a < 0.9f); // Espera até que o fade in esteja completo
        }
        yield return new WaitForSeconds(0.2f); // Espera mais um pouco

        tPlayer.position = destino.position; // Teleporta o jogador para o ponto de destino
        CameraController.GetInstance().SetNewBounds(destinationBounds); //Atualiza os limites da câmera para o novo destino

        string origin = "", destination = "";
        origin = transform.parent.transform.parent.gameObject.name;   //Armazena o nome do cenário ond está a porta (a cena de origem)
        destination = destinationBounds.transform.parent.gameObject.name; //Armazena o nome do cenário de destino (a cena para onde a porta leva)
        bool isFlashback = !haveFade;
        GameControllerNicolas.GetInstance().BetweenDoorInteraction(origin, destination, isDoor, isFlashback);    //Cuida das coisas entre a transição de portas (como triggers, etc...)

        if (haveFade)
        {
            yield return new WaitForSeconds(0.5f); // Espera um pouco antes de continuar
            _fadeController.fadeOut(); // Inicia o fade out
        }

        yield return new WaitForSeconds(0.5f); // Espera um pouco para dar tempo para o jogador se estabilizar na nova posição
        GameControllerNicolas.GetInstance().FinishDoorInteraction(origin, destination, isDoor, isFlashback, playerIdleDirection);    //Finaliza a interação com a porta no controlador do jogo (ativa cenas, triggers, etc...)
    }
}
