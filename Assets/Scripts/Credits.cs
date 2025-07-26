using UnityEngine;

public class Credits : MonoBehaviour
{
    void Update()
    {
        //Quando qualquer tecla for pressionada:
        if (Input.anyKeyDown)
        {
            //Globals.ResetGlobalVariables(); //Reseta as variáveis globais
            SceneTransitionController.GetInstance().LoadScene("Menu"); //Carrega a cena do menu principal
            Destroy(gameObject);
        }
    }
}
