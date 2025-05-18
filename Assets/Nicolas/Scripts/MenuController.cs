using UnityEditor.SearchService;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject menuPrincipal; //Referência ao painel do menu principal
    [SerializeField] private GameObject menuConfiguracoes; //Referência ao painel de configurações

    public void StartGame()
    {
        SceneTransitionController.GetInstance().LoadScene("Present");  //Carrega a cena do jogo
    }

    public void OpenSettings()
    {
        menuPrincipal.SetActive(false); //Desativa o painel do menu principal
        menuConfiguracoes.SetActive(true); //Ativa o painel de configurações
    }

    public void CloseSettings()
    {
        menuConfiguracoes.SetActive(false); //Desativa o painel de configurações
        menuPrincipal.SetActive(true); //Ativa o painel do menu principal
    }

    public void ReturnToMainMenu()
    {
        Globals.ResetGlobalVariables();
        SceneTransitionController.GetInstance().LoadScene("Menu"); //Carrega a cena do menu principal
    }
    
    public void QuitGame()
    {
        Application.Quit(); //Sai do jogo
        Debug.Log("Quit");
    }
}
