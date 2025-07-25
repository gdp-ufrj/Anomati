using UnityEngine;

public class FotoHugo : MonoBehaviour
{
    public GameObject timelineSaiFotoHugo;

    void Update()
    {
        //Quando qualquer tecla for pressionada:
        if (Input.anyKeyDown)
        {
            timelineSaiFotoHugo.SetActive(true);
            //Destrói este objeto para que o script não seja executado novamente
            Destroy(gameObject);
        }
    }
}
