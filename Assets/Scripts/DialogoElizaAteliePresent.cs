using UnityEngine;

public class DialogoElizaAteliePresent : MonoBehaviour
{
    void Start()
    {
        Globals.finishDialogoElizaAteliePresent = true;    //Finaliza o diálogo com Eliza no Ateliê no presente
        Destroy(gameObject);  //Destrói este objeto para que o script não seja executado novamente
    }
}
