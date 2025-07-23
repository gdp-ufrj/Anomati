using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogoTrigger2 : MonoBehaviour
{

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Configurações")]
    [SerializeField] private bool dialogoUnico = false;
    [SerializeField] private bool dialogoAutomatico = false;
    [Tooltip("Se atribuído, será ativado quando o diálogo acabar")]
    [SerializeField] private GameObject proximoGameObject;
    [SerializeField] private GameObject[] proximosGameObjects;

    private bool jogadorPerto2 = false;

    private void Update()
    {
        // dispara no E se for manual
        if (!dialogoAutomatico
            && jogadorPerto2
            && !GerenciadorDeDialogos.GetInstancia().dialogoAtivo)
        {
            IniciarDialogo();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("player")) return;
        jogadorPerto2 = true;

        // dispara automático se marcado
        if (dialogoAutomatico
            && !GerenciadorDeDialogos.GetInstancia().dialogoAtivo)
        {
            IniciarDialogo();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("player"))
            jogadorPerto2 = false;
    }

    private void IniciarDialogo()
    {
        // abre o diálogo
        GerenciadorDeDialogos.GetInstancia().EntrarModoDialogo(inkJSON);
        // já começa a coroutine que vai aguardar o fim
        StartCoroutine(AguardarFimEDesencadear());
    }

    private IEnumerator AguardarFimEDesencadear()
    {
        // fica esperando dialogoAtivo virar false
        yield return new WaitWhile(() => GerenciadorDeDialogos.GetInstancia().dialogoAtivo);

        // aqui o diálogo já acabou
        if (proximoGameObject != null)
            proximoGameObject.SetActive(true);
        
        if (proximosGameObjects != null && proximosGameObjects.Length > 0)
        {
            foreach (var go in proximosGameObjects)
            {
                if (go != null)
                    go.SetActive(true);
            }
        }

        if (dialogoUnico)
            Destroy(gameObject);
    }
}
