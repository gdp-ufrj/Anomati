using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class fadeController : MonoBehaviour
{
    public GameObject fumePainel; // Referência ao painel de fade
    public Image fumeImage; // Referência à imagem de fade
    public Color[] corTransicao; // Array de cores para transição
    public float step; // Passo de transição

    public void fadeIn()
    {
        //Ativa o painel de fade
        fumePainel.SetActive(true); 
        StartCoroutine(fadeInCoroutine()); // Inicia a coroutine de fade in

    }

    public void fadeOut()
    {
        StartCoroutine(fadeOutCoroutine()); // Inicia a coroutine de fade out
    }

    IEnumerator fadeInCoroutine()
    {
        for(float i = 0; i <= 1; i += step)
        {
            // Interpola entre as cores de transição
            fumeImage.color = Color.Lerp(corTransicao[0], corTransicao[1], i); 
            yield return new WaitForEndOfFrame(); // Espera um pouco antes de continuar
        }
    }

    IEnumerator fadeOutCoroutine()
    {
        for(float i = 0; i <= 1; i += step)
        {
            // Interpola entre as cores de transição
            fumeImage.color = Color.Lerp(corTransicao[1], corTransicao[0], i);
            yield return new WaitForEndOfFrame(); // Espera um pouco antes de continuar
        }

        // Desativa o painel de fade após a transição
        fumePainel.SetActive(false); 
    }
}
