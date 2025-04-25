using System.Collections;
using UnityEngine;

public class gameController : MonoBehaviour
{
    private fadeController _fadeController; // Referência ao controlador de fade
    

    void Start()
    {
        // Encontra o controlador de fade na cena
        _fadeController = FindAnyObjectByType<fadeController>(); 
        // Inicia o fade Out ao começar o jogo
        _fadeController.fadeOut(); 
    }

    void Update()
    {
        
    }
}
