using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAnimationEvents : MonoBehaviour
{
    public void AfterFadeInEvent()    //Método chamado após o fim da animação de fade in
    {
        SceneManager.LoadScene(Globals.sceneToBeLoaded); //Carrega a cena armazenada na variável
    }

    public void AfterFadeOutEvent()    //Método chamado após o fim da animação de fade out
    {
        SceneTransitionController.GetInstance().bgTransition.transform.parent.gameObject.SetActive(false); //Desativa o canvas de transição
    }
    
    public void AfterClockAnimationEvent()    //Método chamado após o fim da animação do relógio
    {
        SceneTransitionController.GetInstance().FadeOut();
    }
}
