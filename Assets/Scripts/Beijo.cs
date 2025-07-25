using UnityEngine;

public class Beijo : MonoBehaviour
{
    public GameObject timelineSaiBeijo, triggerTimelineDialogoAtelie2025;

    void Update()
    {
        //Quando qualquer tecla for pressionada:
        if (Input.anyKeyDown)
        {
            timelineSaiBeijo.SetActive(true);
            triggerTimelineDialogoAtelie2025.SetActive(true); //Ativa o trigger do diálogo no Ateliê 2025
            //Destrói este objeto para que o script não seja executado novamente
            Destroy(gameObject);
        }
    }
}
