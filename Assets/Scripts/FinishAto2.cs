using UnityEngine;

public class FinishAto2 : MonoBehaviour
{
    void Start()
    {
        Globals.finishAto2 = true;  //Define o trigger de finalização do ato 2 como verdadeiro
        Destroy(gameObject);  //Destrói este objeto para que o script não seja executado novamente
    }
}
