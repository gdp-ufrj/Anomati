using UnityEngine;

public class FinishAto1 : MonoBehaviour
{
    void Start()
    {
        Globals.finishAto1 = true;  //Define o trigger de finalização do ato 1 como verdadeiro
        Destroy(gameObject);  //Destrói este objeto para que o script não seja executado novamente
    }
}
