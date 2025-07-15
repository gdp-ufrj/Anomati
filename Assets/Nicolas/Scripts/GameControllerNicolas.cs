using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameControllerNicolas : MonoBehaviour
{
    [SerializeField] private GameObject player, pai, porta_pai;
    [SerializeField] private GameObject pauseMenu;  //Referência ao menu de pausa
    private static GameControllerNicolas instance;
    [HideInInspector] public bool gamePaused = false, canPause = true;   //Flag para controlar se o jogo está pausado ou não

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
            EnablePlayerMovement();
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
                DisablePlayerMovement();
                DisableDadRun();  //Desabilita a perseguição com o pai, se estiver ativa
                gamePaused = true;
                pauseMenu.SetActive(true);  //Ativa o menu de pausa
            }
        }
        else
        {
            pauseMenu.SetActive(false);  //Desativa o menu de pausa
            gamePaused = false;
            EnablePlayerMovement();
            if (Globals.triggerDadRun && !Globals.endDadRun)
            {
                EnableDadRun();  //Reativa a perseguição com o pai, se estiver ativa
            }
        }
    }

    public void EnablePlayerMovement()
    {
        if (player != null)
            player.GetComponent<PlayerController>().canMove = true;  //Habilita o movimento do jogador
    }
    public void DisablePlayerMovement()
    {
        if (player != null)
            player.GetComponent<PlayerController>().canMove = false;  //Desabilita o movimento do jogador
    }
    public void SetIdleDirectionPlayer()
    {
        if (player != null)
            player.GetComponent<PlayerController>().SetIdleDirection();  //Define a direção de idle do jogador
    }
    public void DisableDadRun()
    {
        if (pai != null)
        {
            pai.GetComponent<Pai>().enabled = false;  //Desativa o script do pai
            pai.GetComponent<NavMeshAgent>().enabled = false;  //Desabilita o NavMeshAgent do pai
        }
    }
    public void EnableDadRun()
    {
        if (pai != null)
        {
            pai.GetComponent<Pai>().enabled = true;  //Ativa o pai
            pai.GetComponent<NavMeshAgent>().enabled = true;  //Habilita o NavMeshAgent do pai
        }
    }
    public void ResetDadPosition()
    {
        if (pai != null)
            pai.GetComponent<Pai>().ResetPosition();  //Reseta a posição do pai para a posição original
    }

    public bool CanInteractWithObject(GameObject objeto)    //Método para verificar se o jogador pode interagir com um objeto
    {
        if (objeto.transform.Find("casa_pai") && Globals.triggerDadRun && !Globals.endDadRun)
            return false;    //Se a porta for a do pai e a perseguição estiver ativa, não interage
        return true;
    }

    public string GetInteractionText(GameObject objeto)    //Método para obter o texto de interação de um objeto
    {
        if (objeto.transform.Find("casa_pai") && Globals.triggerDadRun && !Globals.endDadRun)
            return "Porta Emperrada!";       //Texto específico para a porta do pai durante a perseguição

        if (objeto.CompareTag("door"))
            return "Abrir";

        return "Interagir";
    }


    public void FinishDoorInteraction(string origin, string destination, bool isDoor)    //Aqui acontecerá checagens de triggers após a transição de porta, como ativar cenas, diálogos, etc...
    {
        Debug.Log("FinishDoorInteraction called. From: " + origin + " To: " + destination);
        if (isDoor)     //Se a interação tiver sido realmente com uma porta
        {
            if (origin == Globals.GetSceneName(Globals.MapNames.CasaPai))
            {
                if (Globals.triggerDadRun && !Globals.endDadRun)
                {
                    Globals.endDadRun = true;   //Finaliza a perseguição com o pai
                    DisableDadRun();
                }
            }
            if (destination == Globals.GetSceneName(Globals.MapNames.CasaPai))
                if (Globals.triggerDadRun && !Globals.endDadRun)
                    EnableDadRun();

            if (destination == Globals.GetSceneName(Globals.MapNames.Atelie))
            {
                //Testando a ativação do trigger para iniciar a perseguição com o pai (pode ser ativado após um diálogo, evento, etc.)
                if (!Globals.triggerDadRun)
                    Globals.triggerDadRun = true;
            }
        }
        else    //Se a interação com a porta tiver sido ativada de forma manual após algum evento
        {
            if (destination == Globals.GetSceneName(Globals.MapNames.CasaPai))
                if (Globals.triggerDadRun && !Globals.endDadRun)
                    EnableDadRun();
        }

        EnablePlayerMovement();
        canPause = true;
        DoorTransitionController.GetInstance().isTransitioning = false;
    }


    public void ResetDadRun()
    {
        canPause = false;
        DisableDadRun();
        DisablePlayerMovement();
        StartCoroutine(ResetDadRunIE());  //Inicia a coroutine para resetar a perseguição com o pai
    }
    public IEnumerator ResetDadRunIE()
    {
        yield return new WaitForSeconds(2f);    //Espera um pouco
        porta_pai.SendMessage("interacao", false, SendMessageOptions.DontRequireReceiver);   //Inicia a transição para a casa do pai
    }
}
