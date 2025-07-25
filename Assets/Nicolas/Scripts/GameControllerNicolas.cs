using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameControllerNicolas : MonoBehaviour
{
    [SerializeField] private GameObject player, pai, porta_pai;
    [SerializeField] private GameObject pauseMenu, staminaBar;   //Referência para o menu de pausa e barra de stamina
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

        SetDadDefault();  //Define o pai para o estado padrão
        Globals.currentScene = currentScene; //Armazena o nome da cena atual
    }

    void Update()
    {
        //Debug.Log("Player can move: " + player.GetComponent<PlayerController>().canMove);
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
        //Debug.Log("PauseGame called. Game paused: " + gamePaused);
        if (!gamePaused)
        {
            Debug.Log("Game is not paused, attempting to pause. Player can move: " + player.GetComponent<PlayerController>().canMove + ", Can pause: " + canPause);
            if (player.GetComponent<PlayerController>().canMove && canPause)
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
                EnableDadRun(afterPause: true);  //Reativa a perseguição com o pai
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
            player.GetComponent<PlayerController>().SetIdleDirection(0);  //Define a direção de idle do jogador
    }

    public bool CanInteractWithObject(GameObject objeto)    //Método para verificar se o jogador pode interagir com um objeto
    {
        if (objeto.CompareTag("door"))
        {
            if (objeto.transform.parent.transform.parent != null)
            {
                string nomeMapa = objeto.transform.parent.transform.parent.name; //Obtém o nome do mapa da porta interagida
                string nomeObjeto = objeto.name;
                if (nomeMapa == Globals.GetSceneName(Globals.MapNames.Centro2025))
                {
                    if (!Globals.finishAto2)
                    {
                        if (nomeObjeto.Contains("atelie"))    //Se estiver tentando ir para o ateliê
                        {
                            if (!Globals.endDadRun) return false;
                            return true;
                        }
                        else return false;   //Se não for a porta do ateliê, não interage
                    }
                    else    //Se terminou o ato 2
                    {
                        if (nomeObjeto.Contains("pai"))    //Se estiver tentando ir para a casa do pai
                        {
                            if (!Globals.dialogoCasaPai2025) return false;
                            return true;
                        }
                        else if (nomeObjeto.Contains("hugo"))   //Se estiver tentando ir para a casa do Hugo
                        {
                            if (!Globals.dialogoPai2025) return false;
                            return true;
                        }
                        else return false;
                    }
                }
                if (nomeMapa == Globals.GetSceneName(Globals.MapNames.CasaPai2000) && Globals.triggerDadRun && !Globals.endDadRun)
                    if (!nomeObjeto.Contains("fundos"))   //Se não for a porta dos fundos
                        return false;
                if (nomeMapa == Globals.GetSceneName(Globals.MapNames.Atelie2000) && !Globals.finishAto1)
                    return false;   //Se o objeto for a porta do ateliê de 2025 e o ato 1 não tiver sido finalizado, não interage
            }
        }

        return true;
    }

    public string GetInteractionText(GameObject objeto)    //Método para obter o texto de interação de um objeto
    {
        if (objeto.CompareTag("door"))
        {
            if (objeto.transform.parent.transform.parent != null)
            {
                string nomeMapa = objeto.transform.parent.transform.parent.name; //Obtém o nome do mapa da porta interagida
                string nomeObjeto = objeto.name;
                if (nomeMapa == Globals.GetSceneName(Globals.MapNames.Centro2025))
                {
                    if (!Globals.finishAto2)
                    {
                        if (!nomeObjeto.Contains("atelie"))    //Se estiver tentando ir para o ateliê
                        {
                            return "Preciso ir ao Ateliê.";
                        }
                    }
                    else    //Se terminou o ato 2
                    {
                        if (!Globals.dialogoPai2025)   //Se ainda não falou com o pai
                        {
                            if (!nomeObjeto.Contains("pai"))
                                return "Preciso ir à casa do meu pai.";
                        }
                        else
                        {
                            if (!nomeObjeto.Contains("hugo"))
                                return "Preciso ir até a casa do Hugo.";
                        }
                    }
                }

                if (nomeMapa == Globals.GetSceneName(Globals.MapNames.CasaPai2000) && Globals.triggerDadRun && !Globals.endDadRun)
                    if (!nomeObjeto.Contains("fundos"))   //Se não for a porta dos fundos
                        return "Porta Trancada!";
                if (nomeMapa == Globals.GetSceneName(Globals.MapNames.Atelie2000) && !Globals.finishAto1)
                    return "Não quero sair.";
            }

            return "Abrir";    //Texto padrão para portas
        }

        return "Interagir";
    }


    public void BetweenDoorInteraction(string origin, string destination, bool isDoor, bool isFlashback)    //Será chamado durante a transição de porta (durante o fade)
    {
        SetIdleDirectionPlayer();  //Define a direção de idle do jogador
        ResetDadPosition();        //Reseta a posição do pai

        if (isDoor)     //Se a interação tiver sido realmente com uma porta
        {
            if (origin == Globals.GetSceneName(Globals.MapNames.CasaPai2000))
            {
                if (Globals.triggerDadRun && !Globals.endDadRun)
                    DisableSprintSystem();    //Desabilita o sistema de sprint e stamina do jogador
            }
        }
        else    //Se a interação com a porta tiver sido ativada de forma manual após algum evento
        {
            if (!isFlashback)
            {
                if (destination == Globals.GetSceneName(Globals.MapNames.CasaPai2000))
                    if (Globals.triggerDadRun && !Globals.endDadRun)
                        EnableSprintSystem();   //Habilita o sistema de corrida e stamina do jogador
            }
        }
    }

    public void FinishDoorInteraction(string origin, string destination, bool isDoor, bool isFlashback)    //Aqui acontecerá checagens de triggers após a transição de porta, como ativar cenas, diálogos, etc...
    {
        Debug.Log("FinishDoorInteraction called. From: " + origin + " To: " + destination);
        if (isDoor)     //Se a interação tiver sido realmente com uma porta
        {
            if (origin == Globals.GetSceneName(Globals.MapNames.CasaPai2000))
            {
                if (Globals.triggerDadRun && !Globals.endDadRun)
                {
                    Globals.endDadRun = true;   //Finaliza a perseguição com o pai
                    DisableDadRun();
                    SetDadDefault();
                }
            }
        }
        else    //Se a interação com a porta tiver sido ativada de forma manual após algum evento
        {
            if (!isFlashback)
            {
                if (destination == Globals.GetSceneName(Globals.MapNames.CasaPai2000))
                    if (Globals.triggerDadRun && !Globals.endDadRun)
                        EnableDadRun();  //Ativa o pai
            }
        }

        if (!isFlashback)
        {
            if (!Globals.finishAto2)
            {
                if (origin == Globals.GetSceneName(Globals.MapNames.CasaPai2000))
                {
                    Debug.Log("Não quero habilitar o movimento do jogador");
                }
                else if (!Globals.finishDialogoElizaAteliePresent)
                {
                    if (destination == Globals.GetSceneName(Globals.MapNames.Atelie2025))
                        Debug.Log("Não quero habilitar o movimento do jogador");
                    else
                        EnablePlayerMovement();  //Habilita o movimento do jogador
                }
                else if (destination == Globals.GetSceneName(Globals.MapNames.Montanha2000))
                    Debug.Log("Não quero habilitar o movimento do jogador");
                else
                    EnablePlayerMovement();
            }
            else
            {
                EnablePlayerMovement();
            }
            //else   //Se terminou o ato 2
            //{
            //    if (destination == Globals.GetSceneName(Globals.MapNames.CasaPai2025) && !Globals.dialogoPai2025)
            //        Debug.Log("Não quero habilitar o movimento do jogador");
            //    else
            //        EnablePlayerMovement();
            //}
        }
        canPause = true;
        DoorTransitionController.GetInstance().isTransitioning = false;
    }

    //Métodos para a perseguição com o pai:
    public void EnableDadRun(bool afterPause = false)
    {
        if (pai != null)
        {
            pai.GetComponent<NavMeshAgent>().Warp(pai.transform.position);  //Para não dar o bug de teleporte
            pai.GetComponent<Pai>().enabled = true;
            pai.GetComponent<NavMeshAgent>().enabled = true;  //Habilita o NavMeshAgent do pai
            if (!afterPause)
                StartCoroutine(IncreaseDadVelocity());  //Aumenta a velocidade do pai gradualmente
        }
    }
    public void DisableDadRun()
    {
        if (pai != null)
        {
            pai.GetComponent<NavMeshAgent>().Warp(pai.transform.position);  //Para não dar o bug de teleporte
            pai.GetComponent<NavMeshAgent>().enabled = false;  //Desabilita o NavMeshAgent do pai
        }
    }
    private IEnumerator IncreaseDadVelocity()    //Coroutine para aumentar a velocidade do pai gradualmente
    {
        float tempo = 0f, tempoParaAumentar = 1f; //Tempo total para aumentar a velocidade
        float originalSpeed = pai.GetComponent<NavMeshAgent>().speed; //Velocidade original do pai

        pai.GetComponent<NavMeshAgent>().speed = 0f;
        while (tempo < tempoParaAumentar)
        {
            pai.GetComponent<NavMeshAgent>().speed = Mathf.Lerp(0f, originalSpeed, tempo / tempoParaAumentar);
            tempo += Time.deltaTime;
            yield return null;
        }
        pai.GetComponent<NavMeshAgent>().speed = originalSpeed;
    }
    public void ResetDadPosition()
    {
        if (pai != null && pai.GetComponent<Pai>().enabled)
            pai.GetComponent<Pai>().ResetPosition();  //Reseta a posição do pai para a posição original
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
        porta_pai.SendMessage("interacao", new object[] { false, true }, SendMessageOptions.DontRequireReceiver);   //Inicia a transição para a casa do pai
    }

    public void SetDadDefault()
    {
        if (pai != null)
        {
            if (pai.GetComponent<Pai>().enabled)
            {
                pai.GetComponent<Pai>().ResetPosition();  //Reseta a posição do pai para a posição original
                pai.GetComponent<Pai>().enabled = false;  //Desativa o script do pai
            }
            else
            {
                pai.GetComponent<Animator>().SetBool("IsWalking", false);  //Desativa a animação de caminhada do pai
                pai.GetComponent<Animator>().SetFloat("LastInputX", 0f);
                pai.GetComponent<Animator>().SetFloat("LastInputY", -1f);  //Define a direção de idle do pai
            }
        }
    }

    public void EnableSprintSystem()
    {
        if (player != null)
        {
            EnablePlayerMovement();
            Stamina stamina = player.GetComponent<Stamina>();
            if (stamina != null)
            {
                staminaBar.SetActive(true);  //Ativa a barra de stamina na UI
                stamina.enabled = true;  //Habilita o sistema de stamina
                stamina.ResetStamina();  //Reseta a stamina do jogador
                player.GetComponent<PlayerController>().canSprint = true;  //Habilita o sprint do jogador
            }
        }
    }
    public void DisableSprintSystem()
    {
        if (player != null)
        {
            Stamina stamina = player.GetComponent<Stamina>();
            if (stamina != null)
            {
                staminaBar.SetActive(false);  //Desativa a barra de stamina na UI
                stamina.enabled = false;  //Desabilita o sistema de stamina
                player.GetComponent<PlayerController>().canSprint = false;  //Desabilita o sprint do jogador
            }
        }
    }

    public void ChangePlayerSprite(string time)
    {
        if (player == null)
            return;
        if (time == "Past")
        {
            Debug.Log("Mudando sprite do jogador para passado");
        }
        else if (time == "Present")
        {
            Debug.Log("Mudando sprite do jogador para presente");
        }
    }
}
