using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerNicolas : MonoBehaviour
{
    [SerializeField] private GameObject player; //Referência ao jogador
    [SerializeField] private GameObject pauseMenu;  //Referência ao menu de pausa
    private static GameControllerNicolas instance;
    [HideInInspector] public bool gamePaused = false;   //Flag para controlar se o jogo está pausado ou não

    public static GameControllerNicolas GetInstance()
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

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name; //Armazena o nome da cena atual

        if (Globals.firstScene)
        {
            Globals.firstScene = false;
            SceneTransitionController.GetInstance().bgTransition.transform.parent.gameObject.SetActive(false); //Desativa o canvas de transição
            Debug.Log("Primeira cena carregada");
            if (player)
                player.GetComponent<PlayerController>().canMove = true; //Habilita o movimento do jogador
        }
        else
        {
            if (player != null && Globals.lastCameraBounds != "")
            {
                player.transform.position = Globals.lastPlayerPosition;  //Coloca o jogador na última posição armazenada
                Collider2D lastBounds = GameObject.Find(Globals.lastCameraBounds).GetComponent<Collider2D>(); //Encontra o objeto que define os limites da câmera
                CameraController.GetInstance().SetNewBounds(lastBounds); //Coloca a câmera nos limites armazenados
            }
            FadeOut();
        }

        Globals.currentScene = currentScene; //Armazena o nome da cena atual
    }

    public void TimeTravel()
    {
        Globals.lastPlayerPosition = player.transform.position; //Armazena a posição atual do jogador
        Globals.lastCameraBounds = CameraController.GetInstance().GetCurrentBoundsName();

        if (SceneManager.GetActiveScene().name.Contains("Past"))
            SceneTransitionController.GetInstance().LoadScene("Present");  //Carrega a cena do presente
        else if (SceneManager.GetActiveScene().name.Contains("Present"))
            SceneTransitionController.GetInstance().LoadScene("Past");  //Carrega a cena do passado
    }

    public void FadeOut()    //Controla o fade-out da cena com ou sem a animação do relógio
    {
        if (SceneManager.GetActiveScene().name.Contains("Past"))
        {
            if (Globals.currentScene == "Present")
                SceneTransitionController.GetInstance().FadeOut("Past");
            else
                SceneTransitionController.GetInstance().FadeOut();
        }
        else if (SceneManager.GetActiveScene().name.Contains("Present"))
        {
            if (Globals.currentScene == "Past")
                SceneTransitionController.GetInstance().FadeOut("Present");
            else
                SceneTransitionController.GetInstance().FadeOut();
        }
        else
            SceneTransitionController.GetInstance().FadeOut();
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            if (player.GetComponent<PlayerController>().canMove)
            {
                player.GetComponent<PlayerController>().canMove = false;  //Desabilita o movimento do jogador
                gamePaused = true;
                pauseMenu.SetActive(true);  //Ativa o menu de pausa
            }
        }
        else
        {
            pauseMenu.SetActive(false);  //Desativa o menu de pausa
            gamePaused = false;
            player.GetComponent<PlayerController>().canMove = true;  //Habilita o movimento do jogador
        }
    }
}
