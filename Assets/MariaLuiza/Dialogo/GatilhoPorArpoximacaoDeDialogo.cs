using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogoTrigger2: MonoBehaviour
{
    private bool jogadorPerto; // Flag para verificar se o jogador está dentro do trigger

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON; // Referência ao arquivo JSON do Ink


    private void Awake()
    {
        jogadorPerto = false; // Inicializa a flag como falsa
        Debug.Log("Longe");
    }
    private void Update()
    {
       if (jogadorPerto && !GerenciadorDeDialogos.GetInstancia().dialogoAtivo)
        {
                GerenciadorDeDialogos.GetInstancia().EntrarModoDialogo(inkJSON);
        }
    } 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verifica se o objeto que entrou no trigger é o jogador
        if (collider.gameObject.CompareTag("player"))
        {
            Debug.Log("Chegou perto");
            jogadorPerto = true; // Define a flag como verdadeira
 
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        // Verifica se o objeto que saiu do trigger é o jogador
        if (collider.gameObject.tag == "Player")
        {
            jogadorPerto = false; // Define a flag como falsa
        }
    }
}
