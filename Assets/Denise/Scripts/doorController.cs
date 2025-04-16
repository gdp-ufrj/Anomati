using System.Collections;
using UnityEngine;

public class doorController : MonoBehaviour
{
    private fadeController _fadeController; // Referência ao controlador de fade

    public Transform tPlayer; // Transform do player
    public Transform destino; // Ponto de destino para onde a porta levará o jogador


    void Start()
    {
        // Encontra o controlador de fade na cena
        _fadeController = FindAnyObjectByType<fadeController>(); 
    }

    public void interacao()
    {
        // Inicia a coroutine para acionar a porta
        StartCoroutine(acionarPorta()); 
    }

    IEnumerator acionarPorta()
    {
        _fadeController.fadeIn(); // Inicia o fade in
        yield return new WaitWhile(() => _fadeController.fumeImage.color.a < 0.9f); // Espera até que o fade in esteja completo
        tPlayer.position = destino.position; // Teleporta o jogador para o ponto de destino
        yield return new WaitForSeconds(0.5f); // Espera um pouco antes de continuar
        _fadeController.fadeOut(); // Inicia o fade out
    }
}
