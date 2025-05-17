using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAnimationEvents : MonoBehaviour
{
    private PlayerController _playerController; //Referência ao controlador do jogador

    public void AfterFadeInEvent()    //Método chamado após o fim da animação de fade in
    {
        SceneManager.LoadScene(Globals.sceneToBeLoaded); //Carrega a cena armazenada na variável
    }

    public void AfterFadeOutEvent()    //Método chamado após o fim da animação de fade out
    {
        SceneTransitionController.GetInstance().bgTransition.SetActive(false);
        SceneTransitionController.GetInstance().bgTransition.transform.parent.gameObject.SetActive(false); //Desativa o canvas de transição
        _playerController = FindAnyObjectByType<PlayerController>(); 
        if (_playerController != null)
        {
            //Debug.Log("Reativando movimento do jogador");
            _playerController.canMove = true; //Habilita o movimento do jogador após o fade out
        }
    }
    
    public void AfterClockAnimationEvent()    //Método chamado após o fim da animação do relógio
    {
        SceneTransitionController.GetInstance().clock.SetActive(false); //Desativa o relógio
        SceneTransitionController.GetInstance().FadeOut();
    }
}
