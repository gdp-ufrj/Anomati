using UnityEngine;
using System.Collections;

public class DialogoTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Configurações")]
    [SerializeField] private bool dialogoUnico = false;
    [SerializeField] private bool dialogoAutomatico = false;
    [Tooltip("Se atribuído, será ativado quando o diálogo acabar")]
    [SerializeField] private GameObject proximoGameObject;

    private bool jogadorPerto = false;

    private void Update()
    {
        // dispara no E se for manual
        if (!dialogoAutomatico
            && jogadorPerto
            && !GerenciadorDeDialogos.GetInstancia().dialogoAtivo
            && Input.GetKeyDown(KeyCode.E))
        {
            IniciarDialogo();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        jogadorPerto = true;

        // dispara automático se marcado
        if (dialogoAutomatico
            && !GerenciadorDeDialogos.GetInstancia().dialogoAtivo)
        {
            IniciarDialogo();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            jogadorPerto = false;
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
        // espera o diálogo terminar
        yield return new WaitWhile(() => GerenciadorDeDialogos.GetInstancia().dialogoAtivo);

        // acontece quando o diálogo acaba
        if (proximoGameObject != null)
            proximoGameObject.SetActive(true);

        if (dialogoUnico)
            Destroy(gameObject);
    }
}
