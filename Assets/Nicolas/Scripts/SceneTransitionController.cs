using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionController : MonoBehaviour
{   //É uma classe singleton, ou seja, só pode existir uma instância dela nas cenas
    private static SceneTransitionController instance;
    public GameObject bgTransition, clock;  //Referência às imagens de transição de cena

    public static SceneTransitionController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void FadeOut(string clockAnimation="")
    {
        Color colorBg = bgTransition.GetComponent<Image>().color;
        colorBg.a = 1;
        bgTransition.GetComponent<Image>().color = colorBg;
        if (clockAnimation != "")
        {
            clock.SetActive(true); //Ativa o relógio
            clock.GetComponent<Animator>().Play(clockAnimation); //Executa a animação do relógio
        }
        else
            bgTransition.GetComponent<Animator>().Play("FadeOut");
    }

    public void FadeIn()
    {
        bgTransition.transform.parent.gameObject.SetActive(true); //Ativa o canvas de transição
        bgTransition.GetComponent<Animator>().Play("FadeIn");
    }

    public void LoadScene(string sceneName)
    {
        Globals.sceneToBeLoaded = sceneName;
        FadeIn();    //A cena será carregada pelo evento de animação que ocorrerá após o fade in
    }
}
