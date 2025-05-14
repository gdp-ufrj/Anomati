using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogoTrigger: MonoBehaviour
{
    private bool jogadorPerto; // Flag para verificar se o jogador está dentro do trigger

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON; // Referência ao arquivo JSON do Ink


    private void Awake()
    {
        jogadorPerto = false; // Inicializa a flag como falsa
    }
    private void Update()
    {
       if (jogadorPerto && !GerenciadorDeDialogos.GetInstancia().dialogoAtivo)
        {
            //Caso usar o ImputSystem do Unity:
            //if (InputManager.GetInstance().GetSubmitPressed())
            if(Input.GetKeyDown(KeyCode.E))
            {
                GerenciadorDeDialogos.GetInstancia().EntrarModoDialogo(inkJSON);
            }
        }
    } 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verifica se o objeto que entrou no trigger é o jogador
        if (collider.gameObject.CompareTag("Player"))
        {
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
