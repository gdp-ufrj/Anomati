using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerNicolas : MonoBehaviour {
    private static GameControllerNicolas instance;

    public static GameControllerNicolas GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);    
    }

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name; //Armazena o nome da cena atual

        if (Globals.firstScene)
            Globals.firstScene = false;
        else
        {
            SceneTransitionController.GetInstance().FadeOut();
        }
        
        Globals.currentScene = currentScene; //Armazena o nome da cena atual
    }
}
