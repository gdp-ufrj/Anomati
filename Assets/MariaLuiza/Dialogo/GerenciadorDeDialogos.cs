using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 

public class GerenciadorDeDialogos : MonoBehaviour
{
    [Header("Diálogo UI")]
    [SerializeField] private GameObject painelDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private TextMeshProUGUI nomeFalante; // Referência ao GameObject do nome do falante
    [SerializeField] private Animator portraitAnimator; // Referência ao Animator do retrato
    private Animator layoutAnimator; // Referência ao Animator do layout

    [Header("Choices UI")]
    [SerializeField] private GameObject[] opcoes;
    private TextMeshProUGUI[] textoOpcoes;

    private Story historiaAtual;
    public bool dialogoAtivo {get; private set;}
    private static GerenciadorDeDialogos instancia; 

    private const string SPEAKER_TAG = "speaker"; // Tag para o falante
    private const string PORTRAIT_TAG = "portrait"; // Tag para o retrato
    private const string LAYOUT_TAG = "layout"; // Tag para o layout

    private void Awake()
    {
        if (instancia != null)
        {
            Debug.LogWarning("Há mais de um Grenciador de diálogo na cena");
        }
        instancia = this; 
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

    private void Update()
    {
        
        if (!dialogoAtivo)
            return;

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
    public  void EntrarModoDialogo(TextAsset inkJSON)
    {
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
    }

    private void ContinuarHistoria()
    {
        if(historiaAtual.canContinue)
        {
            textoDialogo.text = historiaAtual.Continue();

            MostrarOpcoes(); // Chama o método para mostrar as opções

            UsarTags(historiaAtual.currentTags); // Chama o método para usar as tags
        }
        else
        {
            SairModoDialogo();
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
        historiaAtual.ChooseChoiceIndex(escolhaIndex);
        ContinuarHistoria();
    }

}
