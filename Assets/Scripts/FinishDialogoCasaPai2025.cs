using UnityEngine;

public class FinishDialogoCasaPai2025 : MonoBehaviour
{
    void Start()
    {
        Globals.dialogoCasaPai2025 = true;  //Define o trigger do diálogo na casa do pai em 2025 como verdadeiro
        Destroy(gameObject);  //Destrói este objeto para que o script não seja executado novamente
    }
}
