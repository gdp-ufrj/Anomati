using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class GerenciadorDeDialogos : MonoBehaviour
{
    [Header("Parametros de Diálogo")]
    [SerializeField] private float tempoEscrever = 2f; // Tempo entre cada letra do diálogo

    [Header("Diálogo UI")]
    [SerializeField] private GameObject painelDialogo;
    [SerializeField] private GameObject IconeContinuar; // Referência ao ícone de continuar
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private TextMeshProUGUI nomeFalante; // Referência ao GameObject do nome do falante
    [SerializeField] private Animator portraitAnimator; // Referência ao Animator do retrato
    private Animator layoutAnimator; // Referência ao Animator do layout

    [Header("Choices UI")]
    [SerializeField] private GameObject[] opcoes;
    private TextMeshProUGUI[] textoOpcoes;

    [Header("Audio")]
    [SerializeField] private EventReference audioEscrita; // Referência ao evento de áudio de escrita
    [Range(1, 5)]
    [SerializeField] private int FrequenciaAudio = 4; // Frequência de reprodução do áudio de escrita
    [Range(-3, 3)]
    [SerializeField] private float minPitch = 0.5f; // Pitch mínimo do áudio de escrita
    [Range(-3, 3)]
    [SerializeField] private float maxPitch = 2f; // Pitch máximo do áudio de escrita
    //[SerializeField] private AudioClip audioSomEscrendo; // Referência ao áudio de continuar
    //[SerializeField] private AudioClip paraAudio; // Referência ao áudio de parar
    //[SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    //[SerializeField] private DialogueAudioInfoSO[] audioInfos;
    //[SerializeField] private bool makePredictable;
    //private DialogueAudioInfoSO currentAudioInfo;
    //private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;
    //private AudioSource audioSource; // Referência ao AudioSource para tocar os sons

    private Story historiaAtual;
    public bool dialogoAtivo {get; private set;}
    private bool dialogoFinalizado = false; // Indica se o diálogo foi finalizado
    private Coroutine coroutineMostrarLinha; // Referência à coroutine que mostra a linha do diálogo
    private static GerenciadorDeDialogos instancia; 

    private const string SPEAKER_TAG = "speaker"; // Tag para o falante
    private const string PORTRAIT_TAG = "portrait"; // Tag para o retrato
    private const string LAYOUT_TAG = "layout"; // Tag para o layout
    public event Action OnDialogoFinalizado;

    private void Awake()
    {
        if (instancia != null)
        {
            Debug.LogWarning("Há mais de um Grenciador de diálogo na cena");
        }
        instancia = this;

        //audioSource = this.gameObject.AddComponent<AudioSource>(); // Adiciona um AudioSource ao GameObject do gerenciador de diálogos
    }

    public static GerenciadorDeDialogos GetInstancia()
    {
        return instancia; // Retorna a instância única do gerenciador de diálogos
    }

    private void Start()
    {
        dialogoAtivo = false;
        painelDialogo.SetActive(false);

        layoutAnimator = painelDialogo.GetComponent<Animator>(); // Pega o componente Animator do painel de diálogo

        textoOpcoes = new TextMeshProUGUI[opcoes.Length]; // Inicializa o array de opções
        int index = 0;
        foreach (GameObject opcao in opcoes) // Para cada opção no array de opções
        {
            textoOpcoes[index] = opcao.GetComponentInChildren<TextMeshProUGUI>(); // Pega o componente TextMeshProUGUI da opção
            index++;
        }
    }

    /*
    private void Update()
    {
        if (!dialogoAtivo) {
            return;
        }
        //if (historiaAtual.currentChoices.Count > 0)
        if (historiaAtual.currentChoices.Count > 0)
        {
            //Caso usar o ImputSystem do Unity:
            //if (InputManager.GetInstance().GetSubmitPressed())
            //Modificar futuramente para usar a mesma tecla de ação do jogador
            // Pressionar espaço seleciona a opção em foco
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject selecionado = EventSystem.current.currentSelectedGameObject;

                if (selecionado != null)
                {
                    var botao = selecionado.GetComponent<UnityEngine.UI.Button>();
                    if (botao != null)
                    {
                        botao.onClick.Invoke();
                    }
                }
            }
        }
        else
        {
            // Sem escolhas, espaço apenas continua o texto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ContinuarHistoria();
            }
        }
    }
    */
    
    private void Update()
    {
        if (!dialogoAtivo)
        {
            return;
        }

        if (dialogoFinalizado 
            && historiaAtual.currentChoices.Count == 0 
            && Input.GetKeyDown(KeyCode.Space))
        {
            ContinuarHistoria();
        }
    } 
     
    public void EntrarModoDialogo(TextAsset inkJSON)
    {
        GameControllerNicolas.GetInstance().DisablePlayerMovement();
        historiaAtual = new Story(inkJSON.text);
        dialogoAtivo = true;
        painelDialogo.SetActive(true);

        nomeFalante.text = "???"; // Limpa o nome do falante
        portraitAnimator.Play("default"); // Reseta o retrato
        layoutAnimator.Play("right"); // Reseta o layout

        ContinuarHistoria();
    }

    private void SairModoDialogo()
    {
        dialogoAtivo = false;
        painelDialogo.SetActive(false);
        textoDialogo.text = "";

        OnDialogoFinalizado?.Invoke();
        //GameControllerNicolas.GetInstance().EnablePlayerMovement();
    }

    private void ContinuarHistoria()
    {
        if(historiaAtual.canContinue)
        {
            if (coroutineMostrarLinha != null)
            {
                StopCoroutine(coroutineMostrarLinha); // Para a coroutine anterior, se houver
            }
            coroutineMostrarLinha = StartCoroutine(MostrarLinha(historiaAtual.Continue())); // Inicia a coroutine para mostrar a linha do diálogo

            UsarTags(historiaAtual.currentTags); // Chama o método para usar as tags
        }
        else
        {
            SairModoDialogo();
        }
    }

    private IEnumerator MostrarLinha(string linha)
    {
        // Limpa o texto do diálogo
        textoDialogo.text = linha; 
        textoDialogo.maxVisibleCharacters = 0;
        // Desativa o ícone de continuar
        IconeContinuar.SetActive(false);
        EsconderOpcoes(); // Chama o método para esconder as opções

        // Reseta o estado de diálogo finalizado
        dialogoFinalizado = false; 

        //Evita capturar o espaço do frame anterior
        yield return new WaitForSeconds(0.01f);

        int contador = 0;

        foreach (char letra in linha.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Se a tecla espaço for pressionada
            {
                textoDialogo.maxVisibleCharacters = linha.Length; // Mostra todo o texto de uma vez
                break; // Sai da coroutine
            }

            textoDialogo.maxVisibleCharacters++; // Adiciona cada letra ao texto do diálogo
            if (!audioEscrita.IsNull && contador % FrequenciaAudio == 0)
            {
                var instancia = RuntimeManager.CreateInstance(audioEscrita);
                float pitchValue = UnityEngine.Random.Range(minPitch, maxPitch);
                //Debug.Log($"Pitch usado: {pitchValue}");
                instancia.setParameterByName("pitchControl", pitchValue);
                instancia.start();
                instancia.release();
            }
            contador++;
            yield return new WaitForSeconds(tempoEscrever); // Espera um pouco antes de adicionar a próxima letra
        }
    
        IconeContinuar.SetActive(true); // Ativa o ícone de continuar
        MostrarOpcoes(); // Chama o método para mostrar as opções
        dialogoFinalizado = true; // Define o estado de diálogo finalizado como verdadeiro
    }

    private void TocarSomEscrita()
    {
       
    }
    private void EsconderOpcoes()
    {
        foreach (GameObject opcaoBotao in opcoes) // Para cada opção no array de opções
        {
            opcaoBotao.SetActive(false); // Desativa a opção
        }
    }

    private void UsarTags(List<string> currentTags)
    {
        foreach (string tag in currentTags) // Para cada tag na lista de tags
        {
            string[] partesTag = tag.Split(':'); // Divide a tag em partes
            if (partesTag.Length != 2) // Se a tag não tem o formato correto, ignora
            {
                Debug.LogWarning("Tag inválida: " + tag);
            }
            string tipoTag = partesTag[0].Trim(); // Pega o tipo da tag
            string valorTag = partesTag[1].Trim(); // Pega o valor da tag

            switch (tipoTag) // Verifica o tipo da tag
            {
                case SPEAKER_TAG:
                    nomeFalante.text = valorTag; // Atualiza o nome do falante
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(valorTag);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(valorTag);
                    break;
                default:
                    Debug.LogWarning("Tag desconhecida: " + tipoTag);
                    break;
            }
        }
    }
    private void MostrarOpcoes()
    {
        List<Choice> opcaoAtual = historiaAtual.currentChoices; // Pega as opções atuais da história

        if(opcaoAtual.Count > opcoes.Length)
        {
            Debug.LogWarning("Mais opções do que o número de opções disponíveis!" + opcaoAtual.Count);
        }

        int index = 0;
        foreach (Choice opcao in opcaoAtual)
        {
            opcoes[index].gameObject.SetActive(true); 
            textoOpcoes[index].text = opcao.text; // Define o texto da opção
            index++;
        }

        for (int i = index; i < opcoes.Length; i++)
        {
            opcoes[i].gameObject.SetActive(false); // Desativa a opção
        }

        StartCoroutine(SelecionarOpcao()); // Inicia a coroutine para selecionar a opção
    }

    private IEnumerator SelecionarOpcao()
    {
        EventSystem.current.SetSelectedGameObject(null); // Desativa o EventSystem atual
        yield return new WaitForEndOfFrame(); // Espera um frame para garantir que o EventSystem seja atualizado
        EventSystem.current.SetSelectedGameObject(opcoes[0].gameObject); // Ativa o EventSystem novamente e seleciona a primeira opção
    }
    public void EscolherOpcao(int escolhaIndex)
    {
        if (dialogoFinalizado) { 

        historiaAtual.ChooseChoiceIndex(escolhaIndex);
        ContinuarHistoria();
        }
    }
}
