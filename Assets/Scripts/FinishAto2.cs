using UnityEngine;

public class FinishAto2 : MonoBehaviour
{
    public GameObject ElizaCentro1, ElizaCentro2, ElizaAtelie, triggerDialogoCasaPai2025;
    void Start()
    {
        Globals.finishAto2 = true;  //Define o trigger de finalização do ato 2 como verdadeiro
        //ElizaCentro1.SetActive(false);
        //ElizaCentro2.SetActive(false);
        //ElizaAtelie.SetActive(false);
        triggerDialogoCasaPai2025.SetActive(true);
        Destroy(gameObject);  //Destrói este objeto para que o script não seja executado novamente
    }
}
