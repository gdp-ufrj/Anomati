using UnityEngine;

public class FinishDialogoPai2025 : MonoBehaviour
{
    public GameObject dialogoTriggerCasaHugo, ElizaCasaPai2025, ElizaCasaHugo2025;
    void Start()
    {
        ElizaCasaHugo2025.SetActive(true);
        ElizaCasaPai2025.SetActive(false);
        Globals.dialogoPai2025 = true;  //Define o trigger do diálogo com o pai em 2025 como verdadeiro
        dialogoTriggerCasaHugo.SetActive(true);
        Destroy(gameObject);  //Destrói este objeto para que o script não seja executado novamente
    }
}
