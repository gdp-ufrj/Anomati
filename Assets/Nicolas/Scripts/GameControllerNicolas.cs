using UnityEngine;

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

    void Start() {
        if (Globals.firstScene)
            Globals.firstScene = false;
        else
            SceneTransitionController.GetInstance().FadeOut();
    }
}
