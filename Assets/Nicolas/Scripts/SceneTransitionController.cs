using UnityEngine;

public class SceneTransitionController : MonoBehaviour
{   //É uma classe singleton, ou seja, só pode existir uma instância dela nas cenas
    private static SceneTransitionController instance;
    public GameObject bgTransition;  //Referência à imagem de transição de cena

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

    public void FadeOut()
    {
        Debug.Log("FadeOut");
        bgTransition.SetActive(true);
        bgTransition.GetComponent<Animator>().Play("FadeOut");
    }
    
    public void FadeIn(){
        bgTransition.SetActive(true);
        bgTransition.GetComponent<Animator>().Play("FadeIn");
    }
    
}
