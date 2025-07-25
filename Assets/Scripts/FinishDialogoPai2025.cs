using UnityEngine;

public class FinishDialogoPai2025 : MonoBehaviour
{
    public GameObject dialogoTriggerCasaHugo;
    void Start()
    {
        Globals.dialogoPai2025 = true;  //Define o trigger do diálogo com o pai em 2025 como verdadeiro
        dialogoTriggerCasaHugo.SetActive(true);
        Destroy(gameObject);  //Destrói este objeto para que o script não seja executado novamente
    }
}
